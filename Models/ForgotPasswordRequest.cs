namespace goOfflineE.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ForgotPasswordRequest" />.
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
