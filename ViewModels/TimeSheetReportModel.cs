using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORP.Administration.Models
{
    public class TimeSheetReportModel
    {
        public List<TimeSheetTaskDetails> TimeSheetTaskDetails { get; set; }

        public DateTime ForDate { get; set; }

        public string StartHour { get; set; }

        public string StartMinute { get; set; }

        public string EndHour { get; set; }

        public string EndMinute { get; set; }

        public string LunchHour { get; set; }

        public string LunchMinute { get; set; }

        public SelectList Users { get; set; }

        public SelectList Projects { get; set; }

        public string SelectedUser { get; set; }

        public string SelectedProject { get; set; }

        public int TimesheetId { get; set; }
    }
}