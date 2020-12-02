namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="Assessment" />.
    /// </summary>
    public class Assessment : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Assessment"/> class.
        /// </summary>
        public Assessment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Assessment"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="assessmentId">The assessmentId<see cref="string"/>.</param>
        public Assessment(string schoolId, string assessmentId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = assessmentId;
        }

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

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the SubjectName.
        /// </summary>
        public string SubjectName { get; set; }
    }
}
