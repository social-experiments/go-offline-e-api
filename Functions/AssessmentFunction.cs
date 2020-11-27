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
    /// Defines the <see cref="AssessmentFunction" />.
    /// </summary>
    public class AssessmentFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _assessmentService.
        /// </summary>
        private readonly IAssessmentService _assessmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentFunction"/> class.
        /// </summary>
        /// <param name="assessmentService">The assessmentService<see cref="IAssessmentService"/>.</param>
        public AssessmentFunction(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        /// <summary>
        /// The Assessment.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssessmentCreateUpdate")]
        public async Task<IActionResult> CreateAssessment(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assessment/teacher")]
            [RequestBodyType(typeof(Assessment), "Create/update teacher assessment")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            Assessment requestData = JsonConvert.DeserializeObject<Assessment>(requestBody);

            await _assessmentService.CreateAssessment(requestData);

            return new OkObjectResult(new { message = " assessment created successfull." });
        }

        /// <summary>
        /// The StudentAssessment.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssessmentStudentCreateUpdate")]
        public async Task<IActionResult> StudentAssessment(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assessment/student")]
            [RequestBodyType(typeof(StudentAssessment), "Create/update student assessment")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            StudentAssessment requestData = JsonConvert.DeserializeObject<StudentAssessment>(requestBody);

            await _assessmentService.CreateStudentAssessment(requestData);

            return new OkObjectResult(new { message = "Student assessment created successfull." });
        }

        /// <summary>
        /// The AssessmentList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssessmentList")]
        [OpenApiOperation("List", "Assessment")]
        public async Task<IActionResult> Assessments(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assessment/{schoolId}/{classId}")] HttpRequest req, string schoolId, string classId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId) || String.IsNullOrEmpty(classId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _assessmentService.GetAssessments(schoolId, classId);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The StudentAssessments.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("StudentAssessmentList")]
        [OpenApiOperation("List", "StudentAssessments")]
        public async Task<IActionResult> StudentAssessments(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assessment/{schoolId}/{classId}/student")] HttpRequest req, string schoolId, string classId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId) || String.IsNullOrEmpty(classId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _assessmentService.GetStudentAssessments(schoolId, classId);

            return new OkObjectResult(response);
        }
    }
}
