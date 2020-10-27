namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IClassService" />.
    /// </summary>
    public interface IClassService
    {
        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="ClassRoom"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateUpdate(ClassRoom model);

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{ClassRoom}}"/>.</returns>
        Task<IEnumerable<ClassRoom>> GetAll(string schoolId = "");

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="classRoomId">The classRoomId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ClassRoom}"/>.</returns>
        Task<ClassRoom> Get(string classRoomId);

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(string classId);
    }
}
