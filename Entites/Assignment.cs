namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="Assignment" />.
    /// </summary>
    public class Assignment : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Assignment"/> class.
        /// </summary>
        public Assignment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Assignment"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="assignmentId">The assignmentId<see cref="string"/>.</param>
        public Assignment(string schoolId, string assignmentId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = assignmentId;
        }

        /// <summary>
        /// Gets or sets the AssignmentName.
        /// </summary>
        public string AssignmentName { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentDescription.
        /// </summary>
        public string AssignmentDescription { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentLevel.
        /// </summary>
        public string AssignmentLevel { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentURL.
        /// </summary>
        public string AssignmentURL { get; set; }

        /// <summary>
        /// Gets or sets the StudentAssigments.
        /// </summary>
        public string StudentAssigments { get; set; }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the SubjectName.
        /// </summary>
        public string SubjectName { get; set; }
    }
}
