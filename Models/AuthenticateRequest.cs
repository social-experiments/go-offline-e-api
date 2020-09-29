namespace goOfflineE.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AuthenticateRequest" />.
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="StudentAuthenticateRequest" />.
    /// </summary>
    public class StudentAuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the EnrolmentNo.
        /// </summary>
        [Required]
        public string EnrolmentNo { get; set; }
    }
}
