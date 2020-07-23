using System;
using System.Collections.Generic;
using System.Text;

namespace Educati.Azure.Function.Api.Entites
{
    public class Student: BaseEntity
    {
        public Student() { }

        public Student(string schoolId, string studentId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = studentId;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string StoragePath { get; set; }
    }
}
