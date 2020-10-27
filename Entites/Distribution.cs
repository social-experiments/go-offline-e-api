namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="Distribution" />.
    /// </summary>
    public class Distribution : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Distribution"/> class.
        /// </summary>
        public Distribution()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Distribution"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="distributionId">The distributionId<see cref="string"/>.</param>
        public Distribution(string schoolId, string distributionId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = distributionId;
        }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the ContentId.
        /// </summary>
        public string ContentId { get; set; }
    }
}
