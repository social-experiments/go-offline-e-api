using AzureFunctions.Extensions.Swashbuckle.Attribute;
using goOfflineE.Azure.Function.Api.Services;
using goOfflineE.Helpers.Attributes;
using goOfflineE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using NSwag.Annotations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace goOfflineE.Functions
{
    public class ClassRoomFunction : AuthenticationFilter
    {
        private readonly IClassService _classService;
        public ClassRoomFunction(IClassService classService)
        {
            _classService = classService;
        }
        [FunctionName("ClassRoomCreateUpdate")]
        [OpenApiOperation("Create/Update", "Class")]
        public async Task<IActionResult> Register(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "class-room")]
            [RequestBodyType(typeof(ClassRoom), "Create/update class")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            ClassRoom requestData = JsonConvert.DeserializeObject<ClassRoom>(requestBody);

            await _classService.CreateUpdate(requestData);

            return new OkObjectResult(new { message = "Create/update class successful." });
        }

        [FunctionName("ClassRoomList")]
        [OpenApiOperation("List", "ClassRoom")]
        [QueryStringParameter("id", "School id", DataType = typeof(string), Required = true)]
        public async Task<IActionResult> ClassRoomList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "class-rooms/{id}")] HttpRequest req, string id)
        {
            var validateStatus = base.AuthorizationStatus(req);
            string schoolId = req.Query["id"];
            schoolId = schoolId ?? id;
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _classService.GetAll(schoolId);

            return new OkObjectResult(response);
        }
    }
}
