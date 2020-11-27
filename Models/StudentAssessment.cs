﻿namespace goOfflineE.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="StudentAssessment" />.
    /// </summary>
    public class StudentAssessment
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentId.
        /// </summary>
        public string AssessmentId { get; set; }

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
        /// Gets or sets the AssessmentAnswers.
        /// </summary>
        public string AssessmentAnswers { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the Attempts.
        /// </summary>
        public int Attempts { get; set; }
    }
}
