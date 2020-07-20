using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educati.Azure.Function.Api.Entites
{
    public class Teacher: BaseEntity
    {
        public Teacher() { }
        public Teacher(string schoolId, string teacherId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = teacherId;
        }
        public string UserId { get; set; }
    }
}
