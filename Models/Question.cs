﻿namespace goOfflineE.Models
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
        public Dictionary<int, string> QuestionOptions { get; set; }

        /// <summary>
        /// Gets or sets the QuestionType.
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the OptionAnswer.
        /// </summary>
        public int OptionAnswer { get; set; }

        /// <summary>
        /// Gets or sets the ShortAnswer.
        /// </summary>
        public string ShortAnswer { get; set; }

        /// <summary>
        /// Gets or sets the QuestionImagePath.
        /// </summary>
        public string QuestionImagePath { get; set; }

        /// <summary>
        /// Gets or sets the MatchColumns.
        /// </summary>
        public Dictionary<string, List<MatchColumn>> MatchColumns { get; set; }

        /// <summary>
        /// Gets or sets the Active.
        /// </summary>
        public bool? Active { set; get; }
    }

    /// <summary>
    /// Defines the <see cref="MatchColumn" />.
    /// </summary>
    public class MatchColumn
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the ImagePath.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsAzurePath.
        /// </summary>
        public bool IsAzurePath { get; set; } = true;
    }
}
