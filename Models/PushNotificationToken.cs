namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="PushNotificationToken" />.
    /// </summary>
    public class PushNotificationToken
    {
        /// <summary>
        /// Gets or sets the RefreshToken.
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
    }
}
