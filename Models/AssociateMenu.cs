namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="AssociateMenu" />.
    /// </summary>
    public class AssociateMenu
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the MenuName.
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the RoleName.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Active.
        /// </summary>
        public bool Active { get; set; }
    }
}
