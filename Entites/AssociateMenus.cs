namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="AssociateMenus" />.
    /// </summary>
    public class AssociateMenus : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssociateMenus"/> class.
        /// </summary>
        public AssociateMenus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociateMenus"/> class.
        /// </summary>
        /// <param name="roleName">The roleName<see cref="string"/>.</param>
        /// <param name="rowKey">The rowKey<see cref="string"/>.</param>
        public AssociateMenus(string roleName, string rowKey)
        {
            this.PartitionKey = roleName;
            this.RowKey = rowKey;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the MenuName.
        /// </summary>
        public string MenuName { get; set; }
    }
}
