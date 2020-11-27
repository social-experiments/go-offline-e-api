namespace goOfflineE.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Assessment" />.
    /// </summary>
    public class Assessment
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
        /// Gets or sets the AssessmentTitle.
        /// </summary>
        public string AssessmentTitle { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentDescription.
        /// </summary>
        public string AssessmentDescription { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentQuiz.
        /// </summary>
        public string AssessmentQuiz { get; set; }

       
    }
}
