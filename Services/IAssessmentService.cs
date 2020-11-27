namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAssessmentService" />.
    /// </summary>
    public interface IAssessmentService
    {
        /// <summary>
        /// The CreateAssessment.
        /// </summary>
        /// <param name="model">The model<see cref="Assessment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateAssessment(Assessment model);

        /// <summary>
        /// The CreateStudentAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateStudentAssessment(StudentAssessment model);

        /// <summary>
        /// The GetAssignments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{TeacherAssignment}}"/>.</returns>
        Task<IEnumerable<Assessment>> GetAssessments(string schoolId, string classId);

        /// <summary>
        /// The GetStudentAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentAssessment}}"/>.</returns>
        Task<IEnumerable<StudentAssessment>> GetStudentAssessments(string schoolId, string classId);
    }
}
