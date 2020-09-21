namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="Course" />.
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class.
        /// </summary>
        /// <param name="categoryName">The categoryName<see cref="string"/>.</param>
        /// <param name="courseId">The courseId<see cref="string"/>.</param>
        public Course(string categoryName, string courseId)
        {
            this.PartitionKey = categoryName;
            this.RowKey = courseId;
        }
    }
}
