namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="AssessmentShare" />.
    /// </summary>
    public class AssessmentShare
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentId.
        /// </summary>
        public string AssessmentId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
