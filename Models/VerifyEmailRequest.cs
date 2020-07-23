using System.ComponentModel.DataAnnotations;

namespace Educati.Azure.Function.Api.Models
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}