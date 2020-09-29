namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IPowerBIService" />.
    /// </summary>
    public interface IPowerBIService
    {
        /// <summary>
        /// The GetPowerBIAccessToken.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{PowerBIResponse}}"/>.</returns>
        Task<IEnumerable<PowerBIResponse>> GetPowerBIAccessToken();
    }
}
