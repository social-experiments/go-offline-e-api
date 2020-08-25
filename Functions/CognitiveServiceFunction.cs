using goOfflineE.Models;
using goOfflineE.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace goOfflineE.Functions
{
    public class CognitiveServiceFunction
    {
        private readonly ICognitiveService _cognitiveService;

        public CognitiveServiceFunction(ICognitiveService cognitiveService)
        {
            _cognitiveService = cognitiveService;
        }

        [FunctionName("TrainStudentPhotoFunction")]
        public void TrainStudentPhoto([BlobTrigger("student-ptotos/{blobName}", Connection = "AzureWebJobsStorage")] Stream imageBlob, string blobName, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {imageBlob.Length} Bytes");

            string[] nameParts = blobName.Split(new char[] { '/' });
            if (nameParts.Length != 3)
            {
                log.LogError("File name is in invalid format, expected schoolId/studentId/PhotoName");
            }

            TrainStudentFace trainStudentFace = new TrainStudentFace()
            {
                SchoolId = nameParts[0],
                StudentId = nameParts[1],
                Photo = imageBlob,
            };
            log.LogInformation($"Start train student model in cognitive services");

            Task.Run(async () => await _cognitiveService.TrainStudentModel(trainStudentFace)).ConfigureAwait(false);
            
            log.LogInformation($"End train student model in cognitive services");
        }

        [FunctionName("ProcessAttendanceFunction")]
        public void ProcessAttendance(
            [BlobTrigger("attendance-ptoto/{blobName}", Connection = "AzureWebJobsStorage")] Stream attendanceBlob, string blobName, ILogger log, 
            [Table("TakmilTable")] ICollector<ConnectionLog> outputTable)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {attendanceBlob.Length} Bytes");

            string[] nameParts = blobName.Split(new char[] { '/' });
            if (nameParts.Length != 2)
            {
                log.LogError("File name is in invalid format, expected schoolId/PhotoName");
            }

            AttendancePhoto attendancePhoto  = new AttendancePhoto()
            {
                SchoolId = nameParts[0],
                Photo = attendanceBlob,
            };
            log.LogInformation($"Start process attendance in cognitive services");

            Task.Run(async () => await _cognitiveService.ProcessAttendance(attendancePhoto)).ConfigureAwait(false);

            log.LogInformation($"End process attendance in cognitive services");
        }
    }
}
