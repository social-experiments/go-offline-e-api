namespace goOfflineE.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Question" />.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the QuestionDescription.
        /// </summary>
        public string QuestionDescription { get; set; }

        /// <summary>
        /// Gets or sets the QuestionOptions.
        /// </summary>
        public List<Dictionary<int, string>> QuestionOptions { get; set; }

        /// <summary>
        /// Gets or sets the QuestionType.
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the Answer.
        /// </summary>
        public string Answer { get; set; }
    }
}
