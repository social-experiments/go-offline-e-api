namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ITeacherService" />.
    /// </summary>
    public interface ITeacherService
    {
        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="TeacherRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateUpdate(TeacherRequest model);

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{TeacherResponse}}"/>.</returns>
        Task<IEnumerable<TeacherResponse>> GetAll(string schoolId);
    }
}
