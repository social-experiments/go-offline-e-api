using goOfflineE.Entites;
using goOfflineE.Models;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface ICognitiveService
    {
        Task TrainStudentModel(QueueDataMessage queueDataMessage);

        Task ProcessAttendance(QueueDataMessage queueDataMessage);
    }
}
