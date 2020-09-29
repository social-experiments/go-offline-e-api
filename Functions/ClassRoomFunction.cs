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
    using System.Web.Http;

    /// <summary>
    /// Defines the <see cref="ClassRoomFunction" />.
    /// </summary>
    public class ClassRoomFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _classService.
        /// </summary>
        private readonly IClassService _classService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRoomFunction"/> class.
        /// </summary>
        /// <param name="classService">The classService<see cref="IClassService"/>.</param>
        public ClassRoomFunction(IClassService classService)
        {
            _classService = classService;
        }

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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

            try
            {
                await _classService.CreateUpdate(requestData);

            }
            catch (HttpResponseException ex)
            {
                return new ConflictObjectResult(ex.Response.Content);

            }
            return new OkObjectResult(new { message = "Create/update class successful." });
        }

        /// <summary>
        /// The ClassRoomList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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
