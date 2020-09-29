namespace goOfflineE.Entites
{
    /// <summary>
    /// Defines the <see cref="ClassRoom" />.
    /// </summary>
    public class ClassRoom : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRoom"/> class.
        /// </summary>
        public ClassRoom()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRoom"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classRoomId">The classRoomId<see cref="string"/>.</param>
        public ClassRoom(string schoolId, string classRoomId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = classRoomId;
        }

        /// <summary>
        /// Gets or sets the ClassRoomName.
        /// </summary>
        public string ClassRoomName { get; set; }

        /// <summary>
        /// Gets or sets the ClassDivision.
        /// </summary>
        public string ClassDivision { get; set; }
    }
}
