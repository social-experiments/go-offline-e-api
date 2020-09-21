namespace goOfflineE.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="School" />.
    /// </summary>
    public class School
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="School"/> class.
        /// </summary>
        public School()
        {
            this.ClassRooms = new List<ClassRoom>();
            this.Teachers = new List<TeacherResponse>();
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Address1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the Address2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Zip.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the SyncDateTime.
        /// </summary>
        public DateTime? SyncDateTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ClassRooms.
        /// </summary>
        public List<ClassRoom> ClassRooms { get; set; }

        /// <summary>
        /// Gets or sets the Teachers.
        /// </summary>
        public List<TeacherResponse> Teachers { get; set; }
    }
}
