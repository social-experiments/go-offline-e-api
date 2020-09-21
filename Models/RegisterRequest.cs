namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="RegisterRequest" />.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the FirstName.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether AcceptTerms.
        /// </summary>
        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }
    }
}
