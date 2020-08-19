using System.ComponentModel.DataAnnotations;

namespace goOfflineE.Models
{
    public class StudentRequest: UserRequest
    {
        [Required]
        public string SchoolId { get; set; }

        [Required]
        public string ClassId { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Zip { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
