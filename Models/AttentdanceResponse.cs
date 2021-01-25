namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="AttentdanceResponse" />.
    /// </summary>
    public class AttentdanceResponse
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ClassRoomId.
        /// </summary>
        public string ClassRoomId { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

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
