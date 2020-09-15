using goOfflineE.Entites;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public class ProfileUpdateRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("confirmedPassword")]
        public string ConfirmPassword { get; set; }

        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
    }
}

