using AzureFunctions.Extensions.Swashbuckle.Attribute;
using goOfflineE.Helpers;
using goOfflineE.Models;
using goOfflineE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
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

        [FunctionName("QueueMessage")]
        public async Task<IActionResult> QueueMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "queue/message")]
            [RequestBodyType(typeof(QueueDataMessage), "Queue Message")] HttpRequest request, ILogger log)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            QueueDataMessage requestData = JsonConvert.DeserializeObject<QueueDataMessage>(requestBody);
            requestData.QueueCreateTime = DateTime.UtcNow;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(SettingConfigurations.AzureWebJobsStorage);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("queue-message");

            await queue.CreateIfNotExistsAsync();
            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(requestData));
            await queue.AddMessageAsync(message);

            return new OkObjectResult(new { message = "Queueed message successful." });
        }

        [FunctionName("ProcessQueueMessage")]
        public void ProcessQueueMessage([QueueTrigger("queue-message", Connection = "AzureWebJobsStorage")] string queueMessage, ILogger log)
        {
            ProcessQueue(queueMessage, log);
        }

        [FunctionName("ProcessPoisonQueueMessage")]
        public void ProcessPoisonQueueMessage([QueueTrigger("queue-message-poison", Connection = "AzureWebJobsStorage")] string queueMessage, ILogger log)
        {
            ProcessQueue(queueMessage, log);
        }

        private void ProcessQueue(string queueMessage, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {queueMessage}");

            QueueDataMessage queuedData = JsonConvert.DeserializeObject<QueueDataMessage>(queueMessage);

            log.LogInformation($"Start processing cognitive services.");
            if (!string.IsNullOrEmpty(queuedData.StudentId))
            {
                Task.Run(async () => await _cognitiveService.TrainStudentModel(queuedData, log)).ConfigureAwait(false);
            }
            else
            {
                Task.Run(async () => await _cognitiveService.ProcessAttendance(queuedData, log)).ConfigureAwait(false);
            }

            log.LogInformation($"End processing cognitive services.");

        }
    }
}
