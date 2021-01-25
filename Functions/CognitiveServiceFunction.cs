namespace goOfflineE.Functions
{
    using AzureFunctions.Extensions.Swashbuckle.Attribute;
    using goOfflineE.Helpers.Attributes;
    using goOfflineE.Models;
    using goOfflineE.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="CognitiveServiceFunction" />.
    /// </summary>
    public class CognitiveServiceFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _cognitiveService.
        /// </summary>
        private readonly ICognitiveService _cognitiveService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CognitiveServiceFunction"/> class.
        /// </summary>
        /// <param name="cognitiveService">The cognitiveService<see cref="ICognitiveService"/>.</param>
        public CognitiveServiceFunction(ICognitiveService cognitiveService)
        {
            _cognitiveService = cognitiveService;
        }

        /// <summary>
        /// The CognitiveTrainStudent.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("CognitiveTrainStudent")]
        public async Task<IActionResult> CognitiveTrainStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "cognitive/train/student")]
            [RequestBodyType(typeof(QueueDataMessage), "CognitiveTrainStudent")] HttpRequest request, ILogger log)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            QueueDataMessage requestData = JsonConvert.DeserializeObject<QueueDataMessage>(requestBody);
            requestData.QueueCreateTime = DateTime.UtcNow;

            await _cognitiveService.TrainStudentModel(requestData, log);

            return new OkObjectResult(new { message = "Trained student model successful." });
        }

        /// <summary>
        /// The CognitiveProcessAttendance.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("CognitiveProcessAttendance")]
        public async Task<IActionResult> CognitiveProcessAttendance(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "cognitive/process/attendance")]
            [RequestBodyType(typeof(QueueDataMessage), "CognitiveProcessAttendance")] HttpRequest request, ILogger log)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            QueueDataMessage requestData = JsonConvert.DeserializeObject<QueueDataMessage>(requestBody);
            requestData.QueueCreateTime = DateTime.UtcNow;

            await _cognitiveService.ProcessAttendance(requestData, log);

            return new OkObjectResult(new { message = "Process attendance successful." });
        }
    }
}
