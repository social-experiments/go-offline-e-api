using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace goOfflineE.Models
{
    public class QueueDataMessage
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("latlong")]
        public string LatLong { get; set; }

        [JsonProperty("schoolId")]
        public string SchoolId { get; set; }

        [JsonProperty("classId")]
        public string ClassId { get; set; }

        [JsonProperty("teacherId")]
        public string TeacherId { get; set; }

        [JsonProperty("studentId")]
        public string StudentId { get; set; }

        [JsonProperty("pictureURLs")]
        public List<string> PictureURLs { get; set; }

        [JsonProperty("pictureTimestamp")]
        public string PictureTimestamp { get; set; }

    }
}
