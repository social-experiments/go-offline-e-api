namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="TenantService" />.
    /// </summary>
    public class TenantService : ITenantService
    {
        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="tenant">The tenant<see cref="Tenant"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task CreateUpdate(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <returns>The <see cref="Task{Tenant}"/>.</returns>
        public Task<Tenant> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Tenant}}"/>.</returns>
        public Task<IEnumerable<Tenant>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
