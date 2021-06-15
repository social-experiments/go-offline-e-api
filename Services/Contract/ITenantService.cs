namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ITenantService" />.
    /// </summary>
    public interface ITenantService
    {
        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Tenant}}"/>.</returns>
        Task<IEnumerable<Tenant>> GetAll();

        /// <summary>
        /// The Get.
        /// </summary>
        /// <returns>The <see cref="Task{Tenant}"/>.</returns>
        Task<Tenant> Get();

        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="tenant">The tenant<see cref="Tenant"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateUpdate(Tenant tenant);
    }
}
