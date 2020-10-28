namespace goOfflineE.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="TeacherAssignment" />.
    /// </summary>
    public class TeacherAssignment
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the SubjectName.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentName.
        /// </summary>
        public string AssignmentName { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentDescription.
        /// </summary>
        public string AssignmentDescription { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentURL.
        /// </summary>
        public string AssignmentURL { get; set; }

        /// <summary>
        /// Gets or sets the StudentAssignments.
        /// </summary>
        public string StudentAssignments { get; set; }
    }
}
