﻿using System;

namespace goOfflineE.Entites
{
    public class Teacher: BaseEntity
    {
        public Teacher() { }
        public Teacher(string schoolId, string userId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = userId;
        }

        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime? SyncDateTime { get; set; }
    }
}
