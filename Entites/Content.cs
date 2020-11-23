namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="Content" />.
    /// </summary>
    public class Content : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        public Content()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        /// <param name="courseCategory">The courseCategory<see cref="string"/>.</param>
        /// <param name="courseId">The courseId<see cref="string"/>.</param>
        public Content(string courseCategory, string courseId)
        {
            this.PartitionKey = courseCategory;
            this.RowKey = courseId;
        }

        /// <summary>
        /// Gets or sets the CourseName.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Gets or sets the CourseLevel.
        /// </summary>
        public string CourseLevel { get; set; }

        public string CourseAssessment { get; set; }
        

        /// <summary>
        /// Gets or sets the CourseDescription.
        /// </summary>
        public string CourseDescription { get; set; }

        /// <summary>
        /// Gets or sets the CourseURL.
        /// </summary>
        public string CourseURL { get; set; }

        /// <summary>
        /// Gets or sets the ThumbnailURL.
        /// </summary>
        public string ThumbnailURL { get; set; }
    }
}
