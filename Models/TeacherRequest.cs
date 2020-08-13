using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace goOfflineE.Models
{
    public class TeacherRequest: UserRequest
    {
        [Required]
        public string SchoolId { get; set; }

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
