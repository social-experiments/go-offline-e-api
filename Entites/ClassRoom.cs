﻿namespace goOfflineE.Entites
{
    public class ClassRoom : BaseEntity
    {
        public ClassRoom() { }

        public ClassRoom(string schoolId, string classRoomId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = classRoomId;
        }

        public string ClassRoomName { get; set; }

        public string ClassDivision { get; set; }

    }
}
