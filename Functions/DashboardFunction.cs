namespace goOfflineE.Functions
{
    using goOfflineE.Helpers.Attributes;
    using goOfflineE.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DashboardFunction" />.
    /// </summary>
    public class DashboardFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _schoolService.
        /// </summary>
        private readonly ISchoolService _schoolService;

        /// <summary>
        /// Defines the _studentService.
        /// </summary>
        private readonly IStudentService _studentService;

        /// <summary>
        /// Defines the _cognitiveService.
        /// </summary>
        private readonly ICognitiveService _cognitiveService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardFunction"/> class.
        /// </summary>
        /// <param name="schoolService">The schoolService<see cref="ISchoolService"/>.</param>
        /// <param name="studentService">The studentService<see cref="IStudentService"/>.</param>
        /// <param name="cognitiveService">The cognitiveService<see cref="ICognitiveService"/>.</param>
        public DashboardFunction(ISchoolService schoolService, IStudentService studentService, ICognitiveService cognitiveService)
        {
            _schoolService = schoolService;
            _studentService = studentService;
            _cognitiveService = cognitiveService;
        }

        /// <summary>
        /// The SchoolList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("DashboardSchoolList")]
        public async Task<IActionResult> DashboardSchoolList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dashboard/schools")] HttpRequest req)
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
        /// The StudentList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("DashboardStudentList")]
        public async Task<IActionResult> DashboardStudentList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dashboard/students")] HttpRequest req)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _studentService.GetAll();

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The AttentdanceList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("DashboardAttentdanceList")]
        public async Task<IActionResult> DashboardAttentdanceList(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dashboard/attendance")] HttpRequest req)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _cognitiveService.GetAttentdance();

            return new OkObjectResult(response);
        }
    }
}
