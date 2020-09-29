namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ProfileUpdateRequest" />.
    /// </summary>
    public class ProfileUpdateRequest
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the FirstName.
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword.
        /// </summary>
        [JsonProperty("confirmedPassword")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
    }
}
