using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using goOfflineE.Helpers.Attributes;
using goOfflineE.Services;
using NSwag.Annotations;
using goOfflineE.Models;
using AzureFunctions.Extensions.Swashbuckle.Attribute;

namespace goOfflineE.Functions
{
    public  class StudentFunction: AuthenticationFilter
    {
        private readonly IStudentService _studentService;
        public StudentFunction(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [FunctionName("StudentCreateUpdate")]
        [OpenApiOperation("Create/Update", "Student")]
        public async Task<IActionResult> Register(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "student")]
            [RequestBodyType(typeof(StudentRequest), "Create/update student")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            StudentRequest requestData = JsonConvert.DeserializeObject<StudentRequest>(requestBody);

            await _studentService.CreateUpdate(requestData);

            return new OkObjectResult(new { message = "Create/update student successful." });
        }

        [FunctionName("StudentList")]
        [OpenApiOperation("List", "Student")]
        //[QueryStringParameter("schoolId", "School id", DataType = typeof(string), Required = true)]
        //[QueryStringParameter("classId", "Class id", DataType = typeof(string), Required = true)]
        public async Task<IActionResult> StudentList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students/{schoolId}/{classId}")] HttpRequest req, string schoolId, string classId)
        {
            var validateStatus = base.AuthorizationStatus(req);
           // string schoolId = req.Query["schoolId"];
            //schoolId = schoolId ?? id;
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _studentService.GetAll(schoolId, classId);

            return new OkObjectResult(response);
        }
    }
}
