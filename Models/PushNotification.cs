namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="PushNotification" />.
    /// </summary>
    public class PushNotification
    {
        /// <summary>
        /// Gets or sets the RecipientDeviceToken.
        /// </summary>
        public string RecipientDeviceToken { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Body.
        /// </summary>
        public string Body { get; set; }
    }
}
