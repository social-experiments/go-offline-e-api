namespace goOfflineE.Services
{
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="SettingService" />.
    /// </summary>
    public class SettingService : ISettingService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        public SettingService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// The GetMenus.
        /// </summary>
        /// <param name="roleName">The roleName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{AssociateMenu}}"/>.</returns>
        public async Task<IEnumerable<AssociateMenu>> GetMenus(string roleName)
        {
            var allMenus = await _tableStorage.GetAllAsync<Entites.AssociateMenus>("AssociateMenu");
            var roleAssociateMenus = allMenus.Where(menu => menu.PartitionKey == roleName.Trim());

            return from menu in roleAssociateMenus
                   where menu.Active.GetValueOrDefault(false)
                   orderby menu.Id
                   select new AssociateMenu
                   {
                       Id = menu.Id,
                       MenuName = menu.MenuName,
                       RoleName = menu.PartitionKey
                   };
        }

        /// <summary>
        /// The UpdateMenus.
        /// </summary>
        /// <param name="roleName">The roleName<see cref="string"/>.</param>
        /// <param name="associateMenus">The associateMenus<see cref="List{AssociateMenu}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task UpdateMenus(string roleName, List<AssociateMenu> associateMenus)
        {
            var allMenus = await _tableStorage.GetAllAsync<Entites.AssociateMenus>("AssociateMenu");
            var roleAssociateMenus = allMenus.Where(user => user.PartitionKey == roleName.Trim());

            foreach (var menu in associateMenus)
            {
                var associateMenu = roleAssociateMenus.FirstOrDefault(m => m.Id == menu.Id);
                if (associateMenu is null)
                {
                    try
                    {
                        var rowKey = Guid.NewGuid().ToString();
                        var newMenu = new Entites.AssociateMenus(menu.RoleName.Trim(), rowKey)
                        {
                            Id = menu.Id,
                            MenuName = menu.MenuName,
                            Active = menu.Active
                        };
                        await _tableStorage.AddAsync("AssociateMenu", newMenu);
                    }
                    catch (Exception ex)
                    {
                        throw new AppException("AssociateMenu Create Error: ", ex.InnerException);
                    }
                }
                else
                {
                    try
                    {
                        associateMenu.Active = menu.Active;
                        await _tableStorage.UpdateAsync("AssociateMenu", associateMenu);
                    }
                    catch (Exception ex)
                    {
                        throw new AppException("AssociateMenu Update Error: ", ex.InnerException);
                    }
                }
            }
        }
    }
}
