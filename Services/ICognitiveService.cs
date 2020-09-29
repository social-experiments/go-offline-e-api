namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ICognitiveService" />.
    /// </summary>
    public interface ICognitiveService
    {
        /// <summary>
        /// The TrainStudentModel.
        /// </summary>
        /// <param name="queueDataMessage">The queueDataMessage<see cref="QueueDataMessage"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task TrainStudentModel(QueueDataMessage queueDataMessage, ILogger log);

        /// <summary>
        /// The ProcessAttendance.
        /// </summary>
        /// <param name="queueDataMessage">The queueDataMessage<see cref="QueueDataMessage"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task ProcessAttendance(QueueDataMessage queueDataMessage, ILogger log);
    }
}
