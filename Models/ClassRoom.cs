namespace goOfflineE.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ClassRoom" />.
    /// </summary>
    public class ClassRoom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRoom"/> class.
        /// </summary>
        public ClassRoom()
        {
            this.Students = new List<StudentResponse>();
        }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the ClassRoomName.
        /// </summary>
        public string ClassRoomName { get; set; }

        /// <summary>
        /// Gets or sets the ClassDivision.
        /// </summary>
        public string ClassDivision { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedBy.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the Students.
        /// </summary>
        public List<StudentResponse> Students { get; set; }
    }
}
