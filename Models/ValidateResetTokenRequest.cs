using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}