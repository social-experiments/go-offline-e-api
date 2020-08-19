using goOfflineE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IPowerBIService
    {
        Task<IEnumerable<PowerBIResponse>> GetPowerBIAccessToken();
    }
}
