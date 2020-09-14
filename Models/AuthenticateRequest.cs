using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class StudentAuthenticateRequest
    {
        [Required]
        public string EnrolmentNo { get; set; }
    }
}