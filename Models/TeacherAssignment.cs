namespace goOfflineE.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="TeacherAssignment" />.
    /// </summary>
    public class TeacherAssignment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherAssignment"/> class.
        /// </summary>
        public TeacherAssignment()
        {
            this.StudentAssignments = new List<StudentAssignment>();
        }

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
        /// Gets or sets the AssignmentLevel.
        /// </summary>
        public string AssignmentLevel { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentDescription.
        /// </summary>
        public string AssignmentDescription { get; set; }

        /// <summary>
        /// Gets or sets the AssignmentURL.
        /// </summary>
        public string AssignmentURL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsOffline.
        /// </summary>
        public bool IsOffline { get; set; }

        /// <summary>
        /// Gets or sets the Active.
        /// </summary>
        public bool? Active { set; get; }

        /// <summary>
        /// Gets or sets the StudentAssignments.
        /// </summary>
        public List<StudentAssignment> StudentAssignments { get; set; }
    }
}
