﻿namespace goOfflineE.Entites
{
    using System;

    /// <summary>
    /// Defines the <see cref="School" />.
    /// </summary>
    public class School : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="School"/> class.
        /// </summary>
        public School()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="School"/> class.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="schoolName">The schoolName<see cref="string"/>.</param>
        public School(string schoolId, string schoolName)
        {
            this.PartitionKey = schoolId;
            this.RowKey = schoolName;
        }

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
        /// Gets or sets the ImageURL.
        /// </summary>
        public string ImageURL { get; set; }

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
