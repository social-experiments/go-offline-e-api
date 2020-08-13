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
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Threading.Tasks;

namespace goOfflineE.Functions
{
    public  class AttendanceFunction: AuthenticationFilter
    {
        private readonly IEmailService _emailService;
        public AttendanceFunction (IEmailService emailService)
        {
            _emailService = emailService;
        }
        [FunctionName("EmailTestFunction")]
        public  async Task<IActionResult> Run(
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
