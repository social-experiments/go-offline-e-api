using System.ComponentModel.DataAnnotations;

namespace Educati.Azure.Function.Api.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}