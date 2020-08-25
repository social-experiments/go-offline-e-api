using goOfflineE.Helpers;
using goOfflineE.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public class CognitiveService : ICognitiveService
    {
        private readonly IFaceClient _faceClient;
        public CognitiveService(IFaceClient faceClient)
        {
            _faceClient = faceClient;

            if (Uri.IsWellFormedUriString(SettingConfigurations.CognitiveServiceEndPoint, UriKind.Absolute))
            {
                faceClient.Endpoint = SettingConfigurations.CognitiveServiceEndPoint;
            }
        }

        public async Task TrainStudentModel(TrainStudentFace trainStudentFace)
        {
            try
            {
                // Define school specific student
                Person student = await _faceClient.PersonGroupPerson.CreateAsync(
                    // Id of the PersonGroup that the person belonged to
                    trainStudentFace.SchoolId,
                    // studentId
                    trainStudentFace.StudentId
                ).ConfigureAwait(false);

                Guid studentId = Guid.Parse(trainStudentFace.StudentId);

                // Add faces
                await _faceClient.PersonGroupPerson.AddFaceFromStreamAsync(
                    trainStudentFace.SchoolId, studentId, trainStudentFace.Photo).ConfigureAwait(false);

                // Train the PersonGroup
                await _faceClient.PersonGroup.TrainAsync(trainStudentFace.SchoolId).ConfigureAwait(false);
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

        // Uploads the image file and calls DetectWithStreamAsync.
        public async Task  ProcessAttendance(AttendancePhoto attendancePhoto)
        {
            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    FaceAttributeType.Age, 
                    FaceAttributeType.Gender
                };

            // Call the Face API.
            try
            {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.
                    IList<DetectedFace> faceList =
                        await _faceClient.Face.DetectWithStreamAsync(
                            attendancePhoto.Photo, true, false, (IList<FaceAttributeType?>)faceAttributes).ConfigureAwait(false);

                IList<Guid> faceIds = faceList.Select(face => face.FaceId.Value).ToArray();

                var results = await _faceClient.Face.IdentifyAsync((IList<Guid?>)faceIds, attendancePhoto.SchoolId, null, 1, 0.5).ConfigureAwait(false);
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
                            var person = await _faceClient.PersonGroupPerson.GetAsync(attendancePhoto.SchoolId, candidateId).ConfigureAwait(false);
                            Console.WriteLine("Identified as {0}", person.Name);
                            var studentId = person.Name;
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
    }
}
