using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}