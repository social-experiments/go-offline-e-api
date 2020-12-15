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
        /// The UpdateAssessmentQuestion.
        /// </summary>
        /// <param name="model">The model<see cref="Question"/>.</param>
        /// <param name="assessmentId">The assessmentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateAssessmentQuestion(Question model, string assessmentId);

        /// <summary>
        /// The CreateStudentAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateStudentAssessment(StudentAssessment model);

        /// <summary>
        /// The AssessmentShare.
        /// </summary>
        /// <param name="model">The model<see cref="AssessmentShare"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task AssessmentShare(AssessmentShare model);

        /// <summary>
        /// The GetAssignments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{TeacherAssignment}}"/>.</returns>
        Task<IEnumerable<Assessment>> GetAssessments(string schoolId);

        /// <summary>
        /// The GetAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Assessment}}"/>.</returns>
        Task<IEnumerable<Assessment>> GetAssessments(string schoolId, string classId, string studentId);

        /// <summary>
        /// The GetAssessmentSubjects.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{string}}"/>.</returns>
        Task<List<string>> GetAssessmentSubjects();

        /// <summary>
        /// The GetStudentAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentAssessment}}"/>.</returns>
        Task<IEnumerable<StudentAssessment>> GetStudentAssessments(string schoolId, string classId, string studentId);
    }
}
