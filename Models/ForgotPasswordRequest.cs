using System.ComponentModel.DataAnnotations;

namespace Educati.Azure.Function.Api.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}