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
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AttendanceFunction" />.
    /// </summary>
    public class AttendanceFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _emailService.
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendanceFunction"/> class.
        /// </summary>
        /// <param name="emailService">The emailService<see cref="IEmailService"/>.</param>
        public AttendanceFunction(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("EmailTestFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "email/Test")]
            [RequestBodyType(typeof(EmailRequest), "Email Test API")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EmailRequest emailRequest = JsonConvert.DeserializeObject<EmailRequest>(requestBody);
            await _emailService.SendAsync(emailRequest);

            return new OkObjectResult("Email triggered function executed successfully.");
        }
    }
}
