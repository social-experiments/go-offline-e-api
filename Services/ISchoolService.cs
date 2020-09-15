namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ISchoolService" />.
    /// </summary>
    public interface ISchoolService
    {
        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="SchoolRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateUpdate(SchoolRequest model);

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{School}}"/>.</returns>
        Task<IEnumerable<School>> GetAll(string schoolId = "");

        /// <summary>
        /// The GetSchool.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{School}"/>.</returns>
        Task<School> Get(string schoolId);

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(string schoolId);
    }
}
