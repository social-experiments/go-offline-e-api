using goOfflineE.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface ICognitiveService
    {
        Task TrainStudentModel(QueueDataMessage queueDataMessage, ILogger log);

        Task ProcessAttendance(QueueDataMessage queueDataMessage, ILogger log);
    }
}
