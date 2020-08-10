using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Educati.Azure.Function.Api.Helpers.Attributes;
using Educati.Azure.Function.Api.Models;
using Educati.Azure.Function.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using NSwag.Annotations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Functions
{
    public class TeacherFunction : AuthenticationFilter
    {
        private readonly ITeacherService _teacherService;
        public TeacherFunction(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        [FunctionName("TeacherCreateUpdate")]
        [OpenApiOperation("Create/Update", "Teacher")]
        public async Task<IActionResult> Register(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "teacher")]
            [RequestBodyType(typeof(TeacherRequest), "Create/update teacher")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            TeacherRequest requestData = JsonConvert.DeserializeObject<TeacherRequest>(requestBody);

            await _teacherService.CreateUpdate(requestData);

            return new OkObjectResult(new { message = "Create/update school successful." });
        }

        [FunctionName("TeacherList")]
        [OpenApiOperation("List", "Teacher")]
        [QueryStringParameter("id", "School id", DataType = typeof(string), Required = true)]
        public async Task<IActionResult> TeacherList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "teachers/{id}")] HttpRequest req, string id)
        {
            var validateStatus = base.AuthorizationStatus(req);
            string schoolId = req.Query["id"];
            schoolId = schoolId ?? id;
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _teacherService.GetAll(schoolId);

            return new OkObjectResult(response);
        }
    }
}
