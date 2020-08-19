using System;
using System.Collections.Generic;
using System.Text;

namespace goOfflineE.Models
{
    public class PowerBIResponse
    {
        public string ReportId { get; set; }
        public string ReportType { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string EmbedUrl { get; set; }

    }
}
