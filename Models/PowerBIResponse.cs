namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="PowerBIResponse" />.
    /// </summary>
    public class PowerBIResponse
    {
        /// <summary>
        /// Gets or sets the ReportId.
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// Gets or sets the ReportType.
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// Gets or sets the TokenType.
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the AccessToken.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the EmbedUrl.
        /// </summary>
        public string EmbedUrl { get; set; }
    }
}
