using goOfflineE.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface ITeacherService
    {
        Task CreateUpdate(TeacherRequest model);

        Task<IEnumerable<TeacherResponse>> GetAll(string schoolId);
    }
}
