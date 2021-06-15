namespace goOfflineE.Services
{
    using AutoMapper;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="TenantService" />.
    /// </summary>
    public class TenantService : ITenantService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Defines the _mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        public TenantService(ITableStorage tableStorage, IMapper mapper)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
        }

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
        /// <param name="tentantId">The tentantId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Tenant}"/>.</returns>
        public async Task<Tenant> Get(string tentantId)
        {
            var tenants = await this.GetAll();
            return tenants.FirstOrDefault(m => m.Id == tentantId);
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Tenant}}"/>.</returns>
        public async Task<IEnumerable<Tenant>> GetAll()
        {
            var dataTentants = await _tableStorage.GetAllAsync<Entites.Tenant>("Tenants");
            var tenants = dataTentants.ToList();
            return _mapper.Map<List<Tenant>>(tenants);
        }
    }
}
