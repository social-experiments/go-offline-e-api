using Educati.Azure.Function.Api.Entites;
using System.ComponentModel.DataAnnotations;

namespace Educati.Azure.Function.Api.Models
{ 
    public class CreateRequest
    {
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}