namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="AssessmentShare" />.
    /// </summary>
    public class AssessmentShare : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentShare"/> class.
        /// </summary>
        public AssessmentShare()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentShare"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="sharedId">The sharedId<see cref="string"/>.</param>
        public AssessmentShare(string schoolId, string sharedId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = sharedId;
        }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentId.
        /// </summary>
        public string AssessmentId { get; set; }
    }
}
