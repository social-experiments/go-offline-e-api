using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace goOfflineE.Entites
{
    public class School: BaseEntity
    {
        public School() { }
        public School(string schoolId, string schoolName)
        {
            this.PartitionKey = schoolId;
            this.RowKey = schoolName;
        }

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
    }
}
