using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Text.Json.Serialization;

namespace Educati.Azure.Function.Api.Entites
{
    public class User: BaseEntity
    {
        public User(string schoolId, string userId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = userId;
        }

        public User() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
