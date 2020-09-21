namespace goOfflineE.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="AccountResponse" />.
    /// </summary>
    public class AccountResponse
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the Created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the Updated.
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsVerified.
        /// </summary>
        public bool IsVerified { get; set; }
    }
}
