namespace goOfflineE.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="QueueDataMessage" />.
    /// </summary>
    public class QueueDataMessage
    {
        /// <summary>
        /// Gets or sets the SchoolId.
        /// </summary>
        [JsonProperty("schoolId")]
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the ClassId.
        /// </summary>
        [JsonProperty("classId")]
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the CourseId.
        /// </summary>
        [JsonProperty("courseId")]
        public string CourseId { get; set; }

        /// <summary>
        /// Gets or sets the TeacherId.
        /// </summary>
        [JsonProperty("teacherId")]
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the StudentId.
        /// </summary>
        [JsonProperty("studentId")]
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the PictureURLs.
        /// </summary>
        [JsonProperty("pictureURLs")]
        public List<string> PictureURLs { get; set; }

        /// <summary>
        /// Gets or sets the PictureTimestamp.
        /// </summary>
        [JsonProperty("pictureTimestamp")]
        public DateTime PictureTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the QueueCreateTime.
        /// </summary>
        [JsonProperty("queueCreateTime")]
        public DateTime QueueCreateTime { get; set; }
    }
}
