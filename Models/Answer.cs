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
        /// Gets or sets the OptionAnswer.
        /// </summary>
        public int OptionAnswer { get; set; }

        /// <summary>
        /// Gets or sets the ShortAnswer.
        /// </summary>
        public string ShortAnswer { get; set; }
    }
}
