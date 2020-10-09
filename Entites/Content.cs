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
        /// <param name="categoryName">The categoryName<see cref="string"/>.</param>
        /// <param name="courseId">The courseId<see cref="string"/>.</param>
        public Content(string categoryName, string courseId)
        {
            this.PartitionKey = categoryName;
            this.RowKey = courseId;
        }

        /// <summary>
        /// Gets or sets the CourseName.
        /// </summary>
        public string CourseName { get; set; }

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
