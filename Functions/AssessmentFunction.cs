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
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assessment/update")]
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

            return new OkObjectResult(new { message = "Assessment created successfull." });
        }

        /// <summary>
        /// The CreateAssessmentQuestion.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <param name="assessmentId">The assessmentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssessmentCreateUpdateQuestion")]
        public async Task<IActionResult> CreateAssessmentQuestion(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assessment/question/{assessmentId}/update")]
            [RequestBodyType(typeof(Question), "Create/update question assessment")] HttpRequest request, string assessmentId)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(assessmentId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            Question requestData = JsonConvert.DeserializeObject<Question>(requestBody);

            await _assessmentService.UpdateAssessmentQuestion(requestData, assessmentId);

            return new OkObjectResult(new { message = "Assessment question update successfull." });
        }

        /// <summary>
        /// The StudentAssessment.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssessmentStudentCreateUpdate")]
        public async Task<IActionResult> StudentAssessment(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assessment/student/update")]
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
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("Assessments")]
        [OpenApiOperation("List", "Assessment")]
        public async Task<IActionResult> Assessments(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assessments/{schoolId}")] HttpRequest req, string schoolId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _assessmentService.GetAssessments(schoolId);

            return new OkObjectResult(response);
        }

        [FunctionName("ClassAssessments")]
        [OpenApiOperation("List", "ClassAssessments")]
        public async Task<IActionResult> ClassAssessments(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assessments/{schoolId}/{classId}/{studentId}")] HttpRequest req, string schoolId, string classId, string studentId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId) || String.IsNullOrEmpty(classId) || String.IsNullOrEmpty(studentId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _assessmentService.GetAssessments(schoolId, classId, studentId);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The StudentAssessments.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("StudentAssessments")]
        [OpenApiOperation("List", "StudentAssessments")]
        public async Task<IActionResult> StudentAssessments(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assessments/{schoolId}/{classId}/student/{studentId}")] HttpRequest req, string schoolId, string classId, string studentId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId) || String.IsNullOrEmpty(classId) || String.IsNullOrEmpty(studentId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _assessmentService.GetStudentAssessments(schoolId, classId, studentId);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The AssessmentShare.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssessmentShare")]
        [OpenApiOperation("Assessment/Share", "Assessment")]
        public async Task<IActionResult> AssessmentShare(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assessment/share")]
            [RequestBodyType(typeof(AssessmentShare), "Assessment share")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            AssessmentShare requestData = JsonConvert.DeserializeObject<AssessmentShare>(requestBody);

            string result;
            try
            {
                result = await _assessmentService.AssessmentShare(requestData);

            }
            catch (HttpResponseException ex)
            {
                return new ConflictObjectResult(ex.Response.Content);

            }
            return new OkObjectResult(new { message = result });
        }
    }
}
