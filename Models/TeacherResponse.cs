using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace goOfflineE.Models
{
    public class TeacherResponse
    {
        public string Id { get; set; }

        public string SchoolId { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
     
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Country { get; set; }
       
        public string State { get; set; }
       
        public string City { get; set; }

        public string Zip { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
