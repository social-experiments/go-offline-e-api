namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="UpdateRequest" />.
    /// </summary>
    public class UpdateRequest
    {
        /// <summary>
        /// Defines the _password.
        /// </summary>
        private string _password;

        /// <summary>
        /// Defines the _confirmPassword.
        /// </summary>
        private string _confirmPassword;

        /// <summary>
        /// Defines the _role.
        /// </summary>
        private string _role;

        /// <summary>
        /// Defines the _email.
        /// </summary>
        private string _email;

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [EnumDataType(typeof(Role))]
        public string Role { get => _role; set => _role = replaceEmptyWithNull(value); }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [EmailAddress]
        public string Email { get => _email; set => _email = replaceEmptyWithNull(value); }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [MinLength(6)]
        public string Password { get => _password; set => _password = replaceEmptyWithNull(value); }

        /// <summary>
        /// Gets or sets the ConfirmPassword.
        /// </summary>
        [Compare("Password")]
        public string ConfirmPassword { get => _confirmPassword; set => _confirmPassword = replaceEmptyWithNull(value); }

        /// <summary>
        /// The replaceEmptyWithNull.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
