using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace goOfflineE.Models
{
    public class QueueDataMessage
    {

        [JsonProperty("schoolId")]
        public string SchoolId { get; set; }

        [JsonProperty("classId")]
        public string ClassId { get; set; }

        [JsonProperty("courseId")]
        public string CourseId { get; set; }

        [JsonProperty("teacherId")]
        public string TeacherId { get; set; }

        [JsonProperty("studentId")]
        public string StudentId { get; set; }

        [JsonProperty("pictureURLs")]
        public List<string> PictureURLs { get; set; }

        [JsonProperty("pictureTimestamp")]
        public DateTime PictureTimestamp { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("queueCreateTime")]
        public DateTime QueueCreateTime { get; set; }

    }
}
