namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="StudentAssessment" />.
    /// </summary>
    public class StudentAssessment : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAssessment"/> class.
        /// </summary>
        public StudentAssessment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAssessment"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="studentAssessmentId">The studentAssessmentId<see cref="string"/>.</param>
        public StudentAssessment(string schoolId, string studentAssessmentId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = studentAssessmentId;
        }

        /// <summary>
        /// Gets or sets the AssessmentId.
        /// </summary>
        public string AssessmentId { get; set; }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the StudentName.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentAnswers.
        /// </summary>
        public string AssessmentAnswers { get; set; }

        
    }
}
