using System.Collections.Generic;

namespace goOfflineE.Models
{
    public class ClassRoom
    {
        public ClassRoom()
        {
            this.Students = new List<StudentResponse>();

        }
        public string ClassId { get; set; }

        public string SchoolId { get; set; }
        public string ClassRoomName { get; set; }
        public string ClassDivision { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<StudentResponse> Students { get; set; }

    }
}
