namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="Attentdance" />.
    /// </summary>
    public class Attentdance : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attentdance"/> class.
        /// </summary>
        public Attentdance()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Attentdance"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        public Attentdance(string schoolId, string studentId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = studentId;
        }

        /// <summary>
        /// Gets or sets the ClassRoomId.
        /// </summary>
        public string ClassRoomId { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the CourseId.
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// Gets or sets the TeacherId.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Present.
        /// </summary>
        public bool Present { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        public string Longitude { get; set; }
    }
}
