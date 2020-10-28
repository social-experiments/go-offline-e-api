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
    /// Defines the <see cref="AssignmentFunction" />.
    /// </summary>
    public class AssignmentFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _assignmentService.
        /// </summary>
        private readonly IAssignmentService _assignmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentFunction"/> class.
        /// </summary>
        /// <param name="assignmentService">The assignmentService<see cref="IAssignmentService"/>.</param>
        public AssignmentFunction(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        /// <summary>
        /// The TeacherAssignment.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssigmentTeacherCreateUpdate")]
        public async Task<IActionResult> TeacherAssignment(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assignment/teacher")]
            [RequestBodyType(typeof(TeacherAssignment), "Create/update teacher assignment")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            TeacherAssignment requestData = JsonConvert.DeserializeObject<TeacherAssignment>(requestBody);

            await _assignmentService.CreateTeacherAssigments(requestData);

            return new OkObjectResult(new { message = "Teacher assignment created successfull." });
        }

        /// <summary>
        /// The StudentAssignment.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssigmentStudentCreateUpdate")]
        public async Task<IActionResult> StudentAssignment(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "assignment/student")]
            [RequestBodyType(typeof(StudentAssignment), "Create/update student assignment")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            StudentAssignment requestData = JsonConvert.DeserializeObject<StudentAssignment>(requestBody);

            await _assignmentService.CreateStudentAssigments(requestData);

            return new OkObjectResult(new { message = "Student assignment created successfull." });
        }

        /// <summary>
        /// The AssignmentList.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="className">The className<see cref="string"/>.</param>
        /// <param name="subjectName">The subjectName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AssignmentList")]
        [OpenApiOperation("List", "Assignment")]
        public async Task<IActionResult> AssignmentList(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assignment/{schoolId}/{classId}")] HttpRequest req, string schoolId, string classId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(schoolId) || String.IsNullOrEmpty(classId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _assignmentService.GetAssignments(schoolId, classId);

            return new OkObjectResult(response);
        }
    }
}
