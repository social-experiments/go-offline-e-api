namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAssignmentService" />.
    /// </summary>
    public interface IAssignmentService
    {
        /// <summary>
        /// The CreateTeacherAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="TeacherAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateTeacherAssigments(TeacherAssignment model);

        /// <summary>
        /// The CreateStudentAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateStudentAssigments(StudentAssignment model);

        /// <summary>
        /// The GetAssignments.
        /// </summary>
        /// <param name="className">The className<see cref="string"/>.</param>
        /// <param name="subjectName">The subjectName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{TeacherAssignment}}"/>.</returns>
        Task<IEnumerable<TeacherAssignment>> GetAssignments(string schoolId);
    }
}
