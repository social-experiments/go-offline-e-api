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
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="SchoolFunction" />.
    /// </summary>
    public class SchoolFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _schoolService.
        /// </summary>
        private readonly ISchoolService _schoolService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolFunction"/> class.
        /// </summary>
        /// <param name="schoolService">The schoolService<see cref="ISchoolService"/>.</param>
        public SchoolFunction(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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

        /// <summary>
        /// The SchoolList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
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

        /// <summary>
        /// The DeleteSchool.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("DeleteSchool")]
        public async Task<IActionResult> DeleteSchool(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "school/{schoolId}/delete")] HttpRequest req, string schoolId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            await _schoolService.Delete(schoolId);

            return new OkObjectResult(new { message = "Delete school successful." });
        }
    }
}
