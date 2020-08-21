using goOfflineE.Models;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IProfileService
    {
        Task<object> Register(RegisterRequest model);
        Task UpdateProfile(ProfileUpdateRequest model);
    }
}
