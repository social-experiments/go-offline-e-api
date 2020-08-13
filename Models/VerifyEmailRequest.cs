using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}