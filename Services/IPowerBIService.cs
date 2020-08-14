using goOfflineE.Azure.Function.Api.Models;
using goOfflineE.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IPowerBIService
    {
        Task<IEnumerable<PowerBIResponse>> GetPowerBIAccessToken();
    }
}
