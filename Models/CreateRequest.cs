namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="CreateRequest" />.
    /// </summary>
    public class CreateRequest
    {
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
        /// Gets or sets the Role.
        /// </summary>
        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        [Required]
        public string CreatedBy { get; set; }
    }
}
