using goOfflineE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IStudentService
    {
        Task CreateUpdate(StudentRequest model);

        Task<IEnumerable<StudentResponse>> GetAll(string schoolId, string classId);
    }
}
