namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ISettingService" />.
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// The GetMenus.
        /// </summary>
        /// <param name="roleId">The roleId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{AssociateMenu}}"/>.</returns>
        Task<IEnumerable<AssociateMenu>> GetMenus(string roleName);

        /// <summary>
        /// The UpdateMenus.
        /// </summary>
        /// <param name="associateMenus">The associateMenus<see cref="List{AssociateMenu}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateMenus(string roleName, List<AssociateMenu> associateMenus);
    }
}
