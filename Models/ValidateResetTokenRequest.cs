namespace goOfflineE.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ValidateResetTokenRequest" />.
    /// </summary>
    public class ValidateResetTokenRequest
    {
        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
