namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="Content" />.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the CourseName.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Gets or sets the CourseDescription.
        /// </summary>
        public string CourseDescription { get; set; }

        /// <summary>
        /// Gets or sets the CourseCategory.
        /// </summary>
        public string CourseCategory { get; set; }

        /// <summary>
        /// Gets or sets the CourseLevel.
        /// </summary>
        public string CourseLevel { get; set; }

        /// <summary>
        /// Gets or sets the CourseURL.
        /// </summary>
        public string CourseURL { get; set; }

        /// <summary>
        /// Gets or sets the ThumbnailURL.
        /// </summary>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
