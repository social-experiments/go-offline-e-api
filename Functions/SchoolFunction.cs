using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Educati.Azure.Function.Api.Helpers.Attributes;
using Educati.Azure.Function.Api.Helpers.Enums;
using Educati.Azure.Function.Api.Models;
using Educati.Azure.Function.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Functions
{
    public class SchoolFunction : AuthenticationFilter
    {
        private readonly ISchoolService _schoolService;
        public SchoolFunction(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [FunctionName("CreateSchool")]
        public async Task<IActionResult> Register(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "school/create")]
            [RequestBodyType(typeof(SchoolRequest), "Create new school")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            SchoolRequest requestData = JsonConvert.DeserializeObject<SchoolRequest>(requestBody);

            await _schoolService.Create(requestData);

            return new OkObjectResult(new { message = "Create school successful." });
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
