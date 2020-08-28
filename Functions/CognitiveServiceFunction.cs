using AzureFunctions.Extensions.Swashbuckle.Attribute;
using goOfflineE.Entites;
using goOfflineE.Models;
using goOfflineE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        

        //[FunctionName("ProcessAttendanceFunction")]
        //public void ProcessAttendance(
        //    [BlobTrigger("attendance-photo/{blobName}", Connection = "AzureWebJobsStorage")] Stream attendanceBlob, string blobName, ILogger log, 
        //    [Table("Attendance")] ICollector<Attentdance> attentdanceData)
        //{
        //    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {attendanceBlob.Length} Bytes");

        //    string[] nameParts = blobName.Split(new char[] { '/' });
        //    if (nameParts.Length != 2)
        //    {
        //        log.LogError("File name is in invalid format, expected schoolId/PhotoName");
        //    }

        //    AttendancePhoto attendancePhoto  = new AttendancePhoto()
        //    {
        //        SchoolId = nameParts[0],
        //        Photo = attendanceBlob,
        //    };
        //    log.LogInformation($"Start process attendance in cognitive services");

        //    Task.Run(async () => await _cognitiveService.ProcessAttendance(attendancePhoto, attentdanceData)).ConfigureAwait(false);

        //    log.LogInformation($"End process attendance in cognitive services");
        //}

        [FunctionName("QueueMessage")]
        public async Task<IActionResult> QueueMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "queue/message")]
            [RequestBodyType(typeof(QueueDataMessage), "Queue Message")] HttpRequest request,
             [Queue("queue-message")] ICollector<QueueDataMessage> attentdanceData)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            QueueDataMessage requestData = JsonConvert.DeserializeObject<QueueDataMessage>(requestBody);

            attentdanceData.Add(requestData);

            return new OkObjectResult(new { message = "Queueed message successful." });
        }

        [FunctionName("ProcessQueueMessage")]
        public void ProcessQueueMessage([QueueTrigger("queue-message", Connection = "AzureWebJobsStorage")] string queueMessage, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {queueMessage}");

            QueueDataMessage queuedData = JsonConvert.DeserializeObject<QueueDataMessage>(queueMessage);

            log.LogInformation($"Start processing cognitive services.");
            if (!string.IsNullOrEmpty(queuedData.StudentId))
            {
                Task.Run(async () => await _cognitiveService.TrainStudentModel(queuedData)).ConfigureAwait(false);
            }
            else
            {
                Task.Run(async () => await _cognitiveService.ProcessAttendance(queuedData)).ConfigureAwait(false);
            }

            log.LogInformation($"End processing cognitive services.");
        }
    }
}
