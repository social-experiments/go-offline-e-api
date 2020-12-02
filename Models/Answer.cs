namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="Answer" />.
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Gets or sets the QuestionId.
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the Attempts.
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Gets or sets the AnswerKey.
        /// </summary>
        public int AnswerKey { get; set; }

        /// <summary>
        /// Gets or sets the AttemptAnswer.
        /// </summary>
        public string AttemptAnswer { get; set; }
    }
}
