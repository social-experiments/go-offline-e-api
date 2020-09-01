using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace goOfflineE.Models
{
    public class AttendancePhoto
    {
        public string SchoolId { get; set; }
        public Stream Photo { get; set; }
    }
}
