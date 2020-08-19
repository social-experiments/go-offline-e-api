using goOfflineE.Entites;
using System;
using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public abstract class UserRequest
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }

        public DateTime? SyncDateTime { get; set; }

        public string CreatedBy { get; set; }
    }
}
