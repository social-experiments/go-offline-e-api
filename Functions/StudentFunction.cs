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
    using Newtonsoft.Json;
    using NSwag.Annotations;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="StudentFunction" />.
    /// </summary>
    public class StudentFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _studentService.
        /// </summary>
        private readonly IStudentService _studentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFunction"/> class.
        /// </summary>
        /// <param name="studentService">The studentService<see cref="IStudentService"/>.</param>
        public StudentFunction(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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

        /// <summary>
        /// The StudentList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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
