namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IStudentService" />.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="StudentRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateUpdate(StudentRequest model);

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentResponse}}"/>.</returns>
        Task<IEnumerable<StudentResponse>> GetAll(string schoolId, string classId);

        /// <summary>
        /// The UpdateStatusToTrainStudentModel.
        /// </summary>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <param name="studentPhotos">The studentPhotos<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateStudentProfile(string studentId, List<string> studentPhotos);

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(string studentId);
    }
}
