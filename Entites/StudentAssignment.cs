namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="StudentAssignment" />.
    /// </summary>
    public class StudentAssignment : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAssignment"/> class.
        /// </summary>
        public StudentAssignment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAssignment"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        public StudentAssignment(string schoolId, string id)
        {
            this.PartitionKey = schoolId;
            this.RowKey = id;
        }

        /// <summary>
        /// Gets or sets the AssignmentId.
        /// </summary>
        public string AssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the StudentName.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentURL.
        /// </summary>
        public string AssignmentURL { get; set; }
    }
}
