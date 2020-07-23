using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educati.Azure.Function.Api.Entites
{
    public class ClassRoom: BaseEntity
    {
        public ClassRoom() { }

        public ClassRoom(string schoolId, string classRoomId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = classRoomId;
        }

        public string ClassRoomName { get; set; }

    }
}
