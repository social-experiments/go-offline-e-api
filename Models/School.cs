using System;
using System.Collections.Generic;

namespace goOfflineE.Models
{
    public class School
    {
        public School()
        {
            this.ClassRooms = new List<ClassRoom>();

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime? SyncDateTime { get; set; }

        public string CreatedBy { get; set; }
        public IEnumerable<ClassRoom> ClassRooms { get; set; }

    }
}
