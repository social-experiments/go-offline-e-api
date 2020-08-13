using goOfflineE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface ISchoolService
    {
        Task CreateUpdate(SchoolRequest model);

        Task<IEnumerable<School>> GetAll(string schoolId = "");
    }
}
