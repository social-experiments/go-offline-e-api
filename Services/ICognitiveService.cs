using goOfflineE.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface ICognitiveService
    {
        Task TrainStudentModel(TrainStudentFace trainStudentFace);

        Task ProcessAttendance(AttendancePhoto attendancePhoto);
    }
}
