namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IContentService" />.
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="Content"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateUpdate(Content model);

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Content}}"/>.</returns>
        Task<IEnumerable<Content>> GetAll();

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Content}}"/>.</returns>
        Task<IEnumerable<Content>> GetAll(string schoolId, string classId);

        /// <summary>
        /// The ContentDistribution.
        /// </summary>
        /// <param name="model">The model<see cref="Distribution"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task ContentDistribution(Distribution model);
    }
}
