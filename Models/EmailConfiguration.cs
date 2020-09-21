namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="EmailConfiguration" />.
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// Gets or sets the From.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the SmtpServer.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets the Port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }
    }
}
