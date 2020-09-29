namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="UserRequest" />.
    /// </summary>
    public abstract class UserRequest
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

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
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AcceptTerms.
        /// </summary>
        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }

        /// <summary>
        /// Gets or sets the SyncDateTime.
        /// </summary>
        public DateTime? SyncDateTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
