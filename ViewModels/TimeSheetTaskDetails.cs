using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORP.Administration.Models
{
    public class TimeSheetTaskDetails
    {
        public string TaskName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Username { get; set; }
        public string ProjectCode { get; set; }
        public int TaskId { get; set; }
    }
}