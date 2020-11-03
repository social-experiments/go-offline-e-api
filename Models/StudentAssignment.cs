namespace goOfflineE.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="StudentAssignment" />.
    /// </summary>
    public class StudentAssignment
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentId.
        /// </summary>
        public string AssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the StudentName.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentURL.
        /// </summary>
        public string AssignmentURL { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
