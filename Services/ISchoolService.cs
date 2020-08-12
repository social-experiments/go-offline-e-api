using Educati.Azure.Function.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Services
{
    public interface ISchoolService
    {
        Task CreateUpdate(SchoolRequest model);

        Task<IEnumerable<School>> GetAll(string schoolId = "");
    }
}
