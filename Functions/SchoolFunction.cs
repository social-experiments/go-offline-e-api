using AzureFunctions.Extensions.Swashbuckle.Attribute;
using goOfflineE.Helpers.Attributes;
using goOfflineE.Helpers.Enums;
using goOfflineE.Models;
using goOfflineE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace goOfflineE.Functions
{
    public class SchoolFunction : AuthenticationFilter
    {
        private readonly ISchoolService _schoolService;
        public SchoolFunction(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [FunctionName("SchoolCreateUpdate")]
        public async Task<IActionResult> Register(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "school")]
            [RequestBodyType(typeof(SchoolRequest), "Create/update school")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            SchoolRequest requestData = JsonConvert.DeserializeObject<SchoolRequest>(requestBody);

            await _schoolService.CreateUpdate(requestData);

            return new OkObjectResult(new { message = "Create/update school successful." });
        }

        [FunctionName("SchoolList")]
        public async Task<IActionResult> SchoolList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "schools")] HttpRequest req)
        {
           var validateStatus = base.AuthorizationStatus(req);
           if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _schoolService.GetAll();

            return new OkObjectResult(response);
        }
    }
}
