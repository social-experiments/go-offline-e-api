using System.ComponentModel.DataAnnotations;

namespace Educati.Azure.Function.Api.Models
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}