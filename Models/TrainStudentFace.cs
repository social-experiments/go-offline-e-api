namespace goOfflineE.Models
{
    using System.IO;

    /// <summary>
    /// Defines the <see cref="TrainStudentFace" />.
    /// </summary>
    public class TrainStudentFace
    {
        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the Photo.
        /// </summary>
        public Stream Photo { get; set; }
    }
}
