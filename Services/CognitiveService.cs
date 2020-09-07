namespace goOfflineE.Services
{
    using Aducati.Azure.TableStorage.Repository;
    using goOfflineE.Entites;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    /// <summary>
    /// Defines the <see cref="CognitiveService" />.
    /// </summary>
    public class CognitiveService : ICognitiveService
    {
        /// <summary>
        /// Defines the _faceClient.
        /// </summary>
        private readonly IFaceClient _faceClient;

        /// <summary>
        /// Defines the _azureBlobService.
        /// </summary>
        private readonly IAzureBlobService _azureBlobService;

        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Defines the _studentService.
        /// </summary>
        private readonly IStudentService _studentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CognitiveService"/> class.
        /// </summary>
        /// <param name="faceClient">The faceClient<see cref="IFaceClient"/>.</param>
        /// <param name="azureBlobService">The azureBlobService<see cref="IAzureBlobService"/>.</param>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="studentService">The studentService<see cref="IStudentService"/>.</param>
        public CognitiveService(IFaceClient faceClient, IAzureBlobService azureBlobService, ITableStorage tableStorage, IStudentService studentService)
        {
            _faceClient = faceClient;
            _azureBlobService = azureBlobService;
            _tableStorage = tableStorage;
            _studentService = studentService;

            if (Uri.IsWellFormedUriString(SettingConfigurations.CognitiveServiceEndPoint, UriKind.Absolute))
            {
                faceClient.Endpoint = SettingConfigurations.CognitiveServiceEndPoint;
            }
        }

        /// <summary>
        /// The TrainStudentModel.
        /// </summary>
        /// <param name="queueDataMessage">The queueDataMessage<see cref="QueueDataMessage"/>.</param>
        /// <param name="_log">The _log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task TrainStudentModel(QueueDataMessage queueDataMessage, ILogger _log)
        {
            //Create school group
            _log.LogInformation("TrainStudentModel");
            CreateGroup(queueDataMessage.SchoolId, _log);

            try
            {

                // Define school specific student
                _log.LogInformation($"Define school specific student");
                Person student = await _faceClient.PersonGroupPerson.CreateAsync(
                    // Id of the PersonGroup that the person belonged to
                    queueDataMessage.SchoolId,
                    // studentId
                    queueDataMessage.StudentId
                ).ConfigureAwait(false);

                BlobStorageRequest blobStorage = await _azureBlobService.GetSasUri("students");

                _log.LogInformation($"BlobStorageRequest {blobStorage.StorageUri}");

                foreach (var blobUri in queueDataMessage.PictureURLs)
                {
                    var blobUriBuilder = new System.UriBuilder($"{blobStorage.StorageUri}students/{blobUri}")
                    {
                        Query = blobStorage.StorageAccessToken
                    };
                    _log.LogInformation($"blobUriBuilder { blobUriBuilder.Uri.AbsoluteUri}");

                    await _faceClient.PersonGroupPerson.AddFaceFromUrlAsync(queueDataMessage.SchoolId, student.PersonId, blobUriBuilder.Uri.AbsoluteUri);
                    _log.LogInformation($"AddFaceFromUrlAsync Done");

                }

                _log.LogInformation($"Train the PersonGroup Start {queueDataMessage.SchoolId}");

                // Train the PersonGroup
                await _faceClient.PersonGroup.TrainAsync(queueDataMessage.SchoolId).ConfigureAwait(false);
                await _studentService.UpdateStudentProfile(queueDataMessage.StudentId, queueDataMessage.PictureURLs[0]);

                _log.LogInformation($"Train the PersonGroup Done {queueDataMessage.SchoolId}");
            }
            // Catch and display Face API errors.
            catch (APIErrorException ex)
            {
                _log.LogInformation($"TrainStudentModel APIErrorException: {ex}");
                throw new AppException("APIErrorException: ", ex.InnerException);
            }
            // Catch and display all other errors.
            catch (Exception ex)
            {
                _log.LogInformation($"TrainStudentModel Exception: {ex}");
                throw new AppException("Train Student Cognitive Service Error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The ProcessAttendance.
        /// </summary>
        /// <param name="queueDataMessage">The queueDataMessage<see cref="QueueDataMessage"/>.</param>
        /// <param name="_log">The _log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ProcessAttendance(QueueDataMessage queueDataMessage, ILogger _log)
        {

            // Call the Face API.
            try
            {
                BlobStorageRequest blobStorage = await _azureBlobService.GetSasUri("attendance");
                _log.LogInformation($"APIErrorException BlobStorageRequest: {blobStorage.StorageUri}");

                var blobUriBuilder = new System.UriBuilder($"{blobStorage.StorageUri}attendance/{queueDataMessage.PictureURLs[0]}")
                {
                    Query = blobStorage.StorageAccessToken
                };

                _log.LogInformation($"APIErrorException blobUriBuilder: {blobUriBuilder.Uri.AbsoluteUri}");


                _log.LogInformation($"Start DetectWithUrlAsync");

                IList<DetectedFace> faceList =
                    await _faceClient.Face.DetectWithUrlAsync(blobUriBuilder.Uri.AbsoluteUri).ConfigureAwait(false);

                _log.LogInformation($"End DetectWithUrlAsync: { faceList}");


                IList<Guid?> faceIds = faceList.Select(face => face.FaceId).ToArray();
                _log.LogInformation($"faceIds : { faceIds}");

                _log.LogInformation($"Start IdentifyAsync");

                var results = await _faceClient.Face.IdentifyAsync(faceIds, queueDataMessage.SchoolId, null, 1, 0.5).ConfigureAwait(false);

                _log.LogInformation($"End results");

                IList<string> presentStudetns = new List<string>();

                foreach (var identifyResult in results)
                {
                    _log.LogInformation($"identifyResult = {identifyResult}");

                    if (identifyResult.Candidates.Count() == 0)
                    {
                        _log.LogInformation($"No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        _log.LogInformation($"Get top 1 among all candidates returned");
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        _log.LogInformation($"candidateId: {candidateId}");

                        var person = await _faceClient.PersonGroupPerson.GetAsync(queueDataMessage.SchoolId, candidateId).ConfigureAwait(false);
                        _log.LogInformation($"Identified as: {person.Name}");

                        var studentId = person.Name;
                        presentStudetns.Add(studentId);

                    }
                }

                var allStudents = await _studentService.GetAll(queueDataMessage.SchoolId, queueDataMessage.ClassId);

                foreach (var student in allStudents)
                {
                    var attaintance = new Attentdance(queueDataMessage.SchoolId, Guid.NewGuid().ToString())
                    {
                        StudentId = student.Id,
                        ClassRoomId = queueDataMessage.ClassId,
                        TeacherId = queueDataMessage.TeacherId,
                        CreatedBy = queueDataMessage.TeacherId,
                        UpdatedBy = queueDataMessage.TeacherId,
                        UpdatedOn = DateTime.UtcNow,
                        Timestamp = queueDataMessage.PictureTimestamp,
                        Latitude = queueDataMessage.Latitude,
                        Longitude = queueDataMessage.Longitude,
                        Present = presentStudetns.Contains(student.Id)
                    };
                    _log.LogInformation($"Attentdance AddAsync: {attaintance}");

                    await _tableStorage.AddAsync("Attentdance", attaintance);

                    _log.LogInformation($" Done Attentdance AddAsync: {attaintance}");
                }
            }
            // Catch and display Face API errors.
            catch (APIErrorException ex)
            {
                _log.LogInformation($"APIErrorException Exception: {ex}");

                throw new AppException("APIErrorException: ", ex.InnerException);
            }
            // Catch and display all other errors.
            catch (Exception ex)
            {
                _log.LogInformation($"ProcessAttendance Exception: {ex}");

                throw new AppException("Process Attendance Cognitive Service Error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The GetPictureData.
        /// </summary>
        /// <param name="jsonBlob">The jsonBlob<see cref="Stream"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="QueueDataMessage"/>.</returns>
        private QueueDataMessage GetPictureData(Stream jsonBlob, ILogger log)
        {
            QueueDataMessage result = new QueueDataMessage();
            var serializer = new JsonSerializer();
            using (StreamReader reader = new StreamReader(jsonBlob))
            {
                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    result = serializer.Deserialize<QueueDataMessage>(jsonTextReader);
                }
            }

            log.LogInformation($"Successfully parsed JSON blob");
            return result;
        }

        /// <summary>
        /// The CreateGroup.
        /// </summary>
        /// <param name="personGroupId">The personGroupId<see cref="string"/>.</param>
        /// <param name="_log">The _log<see cref="ILogger"/>.</param>
        private void CreateGroup(string personGroupId, ILogger _log)
        {
            try
            {
                _log.LogInformation($"CreateGroup: {personGroupId}");
                //Create school group
                _faceClient.PersonGroup.CreateAsync(personGroupId, "School Group").GetAwaiter().GetResult();
            }
            catch
            {
                _log.LogInformation($"Already Created : {personGroupId}");
            }
        }
    }
}
