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
        public string TeacherId { get; set; }
        public int Girls { get; set; }
        public int Boys { get; set; }
        public int Total { get; set; }
        public string ImagePath { get; set; }
    }
}
