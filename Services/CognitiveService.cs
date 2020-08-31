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

namespace goOfflineE.Services
{
    public class CognitiveService : ICognitiveService
    {
        private readonly IFaceClient _faceClient;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ITableStorage _tableStorage;

        public CognitiveService(IFaceClient faceClient, IAzureBlobService azureBlobService, ITableStorage tableStorage)
        {
            _faceClient = faceClient;
            _azureBlobService = azureBlobService;
            _tableStorage = tableStorage;

            if (Uri.IsWellFormedUriString(SettingConfigurations.CognitiveServiceEndPoint, UriKind.Absolute))
            {
                faceClient.Endpoint = SettingConfigurations.CognitiveServiceEndPoint;
            }
        }

        public async Task TrainStudentModel(QueueDataMessage queueDataMessage)
        {
            //Create school group
            CreateGroup(queueDataMessage.SchoolId);

            try
            {

                // Define school specific student
                Person student = await _faceClient.PersonGroupPerson.CreateAsync(
                    // Id of the PersonGroup that the person belonged to
                    queueDataMessage.SchoolId,
                    // studentId
                    queueDataMessage.StudentId
                ).ConfigureAwait(false);

                BlobStorageRequest blobStorage = await _azureBlobService.GetSasUri("students");

                foreach (var blobUri in queueDataMessage.PictureURLs)
                {
                    var blobUriBuilder = new System.UriBuilder($"{blobStorage.StorageUri}students/{blobUri}")
                    {
                        Query = blobStorage.StorageAccessToken
                    };

                    await _faceClient.PersonGroupPerson.AddFaceFromUrlAsync(queueDataMessage.SchoolId, student.PersonId, blobUriBuilder.Uri.AbsoluteUri);
                }

                // Train the PersonGroup
                await _faceClient.PersonGroup.TrainAsync(queueDataMessage.SchoolId).ConfigureAwait(false);

            }
            // Catch and display Face API errors.
            catch (APIErrorException ex)
            {
                throw new AppException("APIErrorException: ", ex.InnerException);
            }
            // Catch and display all other errors.
            catch (Exception ex)
            {
                throw new AppException("Train Student Cognitive Service Error: ", ex.InnerException);
            }

        }

        // Uploads the image file and calls DetectWithUrlAsync.
        public async Task ProcessAttendance(QueueDataMessage queueDataMessage)
        {
            // The list of Face attributes to return.
            //IList<FaceAttributeType> faceAttributes =
            //    new FaceAttributeType[]
            //    {
            //        FaceAttributeType.Age,
            //        FaceAttributeType.Gender
            //    };

            // Call the Face API.
            try
            {
                BlobStorageRequest blobStorage = await _azureBlobService.GetSasUri("attendance");
                var blobUriBuilder = new System.UriBuilder($"{blobStorage.StorageUri}attendance/{queueDataMessage.PictureURLs[0]}")
                {
                    Query = blobStorage.StorageAccessToken
                };

                IList<DetectedFace> faceList =
                    await _faceClient.Face.DetectWithUrlAsync(blobUriBuilder.Uri.AbsoluteUri).ConfigureAwait(false);

                IList<Guid?> faceIds = faceList.Select(face => face.FaceId).ToArray();

                var results = await _faceClient.Face.IdentifyAsync(faceIds, queueDataMessage.SchoolId, null, 1, 0.5).ConfigureAwait(false);
                foreach (var identifyResult in results)
                {
                    if (identifyResult.Candidates.Count() == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await _faceClient.PersonGroupPerson.GetAsync(queueDataMessage.SchoolId, candidateId).ConfigureAwait(false);
                        Console.WriteLine("Identified as {0}", person.Name);
                        var studentId = person.Name;
                        var attaintance = new Attentdance(queueDataMessage.SchoolId, Guid.NewGuid().ToString())
                        {
                            StudentId = studentId,
                            ClassRoomId = queueDataMessage.ClassId,
                            TeacherId = queueDataMessage.TeacherId,
                            CreatedBy = queueDataMessage.TeacherId,
                            UpdatedBy = queueDataMessage.TeacherId,
                            UpdatedOn = DateTime.UtcNow,
                            Timestamp = queueDataMessage.PictureTimestamp,
                            Latitude = queueDataMessage.Latitude,
                            Longitude = queueDataMessage.Longitude,
                            Present = true
                        };
                        await _tableStorage.AddAsync("Attentdance", attaintance);
                    }
                }
            }
            // Catch and display Face API errors.
            catch (APIErrorException ex)
            {
                throw new AppException("APIErrorException: ", ex.InnerException);
            }
            // Catch and display all other errors.
            catch (Exception ex)
            {
                throw new AppException("Process Attendance Cognitive Service Error: ", ex.InnerException);
            }
        }

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

        private void CreateGroup(string personGroupId)
        {
            try
            {
                //Create school group
                _faceClient.PersonGroup.CreateAsync(personGroupId, "School Group").GetAwaiter().GetResult();
            }
            catch
            {

            }

        }
    }
}
