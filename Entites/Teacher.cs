namespace goOfflineE.Entites
{
    using System;

    /// <summary>
    /// Defines the <see cref="Teacher" />.
    /// </summary>
    public class Teacher : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Teacher"/> class.
        /// </summary>
        public Teacher()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Teacher"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="userId">The userId<see cref="string"/>.</param>
        public Teacher(string schoolId, string userId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = userId;
        }

        /// <summary>
        /// Gets or sets the Gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the DOB.
        /// </summary>
        public DateTime? DOB { get; set; }

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
    }
}
