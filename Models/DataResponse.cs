namespace goOfflineE.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="DataResponse" />.
    /// </summary>
    public class DataResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataResponse"/> class.
        /// </summary>
        public DataResponse()
        {
            this.Schools = new List<School>();
            this.CourseContent = new List<Content>();
            this.AssessmentCategory = new List<string>();
            this.AssociateMenu = new List<string>();
        }

        /// <summary>
        /// Gets or sets the Schools.
        /// </summary>
        public List<School> Schools { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentCategory.
        /// </summary>
        public List<string> AssessmentCategory { get; set; }

        /// <summary>
        /// Gets or sets the AssociateMenu.
        /// </summary>
        public List<string> AssociateMenu { get; set; }

        /// <summary>
        /// Gets or sets the CourseContent.
        /// </summary>
        public List<Content> CourseContent { get; set; }
    }
}
