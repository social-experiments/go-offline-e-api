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
    /// Defines the <see cref="TeacherFunction" />.
    /// </summary>
    public class TeacherFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _teacherService.
        /// </summary>
        private readonly ITeacherService _teacherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherFunction"/> class.
        /// </summary>
        /// <param name="teacherService">The teacherService<see cref="ITeacherService"/>.</param>
        public TeacherFunction(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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

        /// <summary>
        /// The TeacherList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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
