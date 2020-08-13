using goOfflineE.Models;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IProfileService
    {
        Task UpdateProfile(ProfileUpdateRequest model);
    }
}
