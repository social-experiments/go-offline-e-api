using Educati.Azure.Function.Api.Models;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Services
{
    public interface IProfileService
    {
        Task UpdateProfile(ProfileUpdateRequest model);
    }
}
