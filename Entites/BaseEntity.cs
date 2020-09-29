namespace goOfflineE.Entites
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// Defines the <see cref="BaseEntity" />.
    /// </summary>
    public abstract class BaseEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the Active.
        /// </summary>
        public bool? Active { set; get; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedBy.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedOn.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
