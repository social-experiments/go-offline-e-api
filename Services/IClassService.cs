using goOfflineE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IClassService
    {
        Task CreateUpdate(ClassRoom model);

        Task<IEnumerable<ClassRoom>> GetAll(string schoolId = "");
    }
}
