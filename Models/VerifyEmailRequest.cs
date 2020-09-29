namespace goOfflineE.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="VerifyEmailRequest" />.
    /// </summary>
    public class VerifyEmailRequest
    {
        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
