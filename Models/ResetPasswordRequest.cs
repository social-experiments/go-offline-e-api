namespace goOfflineE.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ResetPasswordRequest" />.
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword.
        /// </summary>
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
