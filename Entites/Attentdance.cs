namespace goOfflineE.Entites
{
    public class Attentdance : BaseEntity
    {
        public Attentdance() { }

        public Attentdance(string schoolId, string studentId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = studentId;
        }

        public string ClassRoomId { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }

        public string TeacherId { get; set; }
        public bool Present { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
