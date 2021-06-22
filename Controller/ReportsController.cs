using ORP.Administration.Infrastructure;
using ORP.Administration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ORP.Data;
using System.Data.Entity;
using ORP.Administration.Enums;
using ORP.Administration.Helpers;
using System.Text;

namespace ORP.Administration.Controllers
{
    [CustomAuthorize(Roles = "Administrator")]
    public class ReportsController : Controller
    {
        
        public ActionResult TimeSheetGeneration()
        {
            TimeSheetReportModel timeSheetReportModel = new TimeSheetReportModel();
            timeSheetReportModel.Projects = new SelectList(GetProjects(), "Value", "Text");
            timeSheetReportModel.Users= new SelectList(GetUsers(), "Value", "Text");
            return View(timeSheetReportModel);
        }

        [HttpPost]
        public ActionResult TimeSheetGeneration(TimeSheetReportModel model)
        {
            if(ModelState.IsValid)
            {
                using (ORPEntities ctx = new ORPEntities())
                {
                    var timeSheetDetail = new TimeSheetDetail()
                    {
                        StartTime = model.StartHour+":"+model.StartMinute,
                        EndTime = model.EndHour+":"+model.EndMinute,
                        LunchStartTime = model.LunchHour+":"+model.LunchMinute,
                        ForDate = model.ForDate,
                        ProjectID = Convert.ToInt32(model.SelectedProject),
                        UserID = Convert.ToInt32(model.SelectedUser)
                    };
                    ctx.TimeSheetDetails.Add(timeSheetDetail);
                    ctx.SaveChanges();
                    if (timeSheetDetail.TimeSheetID>0)
                    {
                        List<string> intervals= SplitWithInterval(Convert.ToDateTime(timeSheetDetail.StartTime), Convert.ToDateTime(timeSheetDetail.EndTime),
                            Convert.ToDateTime(timeSheetDetail.LunchStartTime), Convert.ToDateTime(timeSheetDetail.LunchStartTime).AddMinutes(60), 45);
                        List<TaskDetail> details = new List<TaskDetail>();
                        int taskCount = 1;
                        foreach(string interval in intervals)
                        {
                            string[] timings = interval.Split('-');
                            var taskDetail = new TaskDetail()
                            {
                                Name = (Convert.ToDateTime(timings[0]).AddMinutes(60) == Convert.ToDateTime(timings[1]))?"Lunch Time" :"Task" +taskCount,
                                TimeSheetID = timeSheetDetail.TimeSheetID,
                                StartTime = timings[0],
                                EndTime = timings[1]
                            };
                            details.Add(taskDetail);
                            taskCount++;
                        }
                        ctx.TaskDetails.AddRange(details);
                        ctx.SaveChanges();
                        model.TimesheetId = timeSheetDetail.TimeSheetID;
                    }
                }
                return Json(model);
            }
            return View(model);
        }

        public ActionResult ViewTimeSheet(int timesheetId)
        {
            TimeSheetReportModel timeSheetReportModel = GetTimeSheets(timesheetId);
            return View(timeSheetReportModel);
        }

        private List<SelectListItem> GetUsers()
        {
            using (ORPEntities ctx = new ORPEntities())
            {
                var model = (from users in ctx.InternalUsers
                             where users.IsAdmin == false
                             select new SelectListItem
                             {
                                 Value = users.UserId.ToString(),
                                 Text = users.FirstName + " " + users.LastName
                             }).ToList();
                return model;
            }
        }

        private List<SelectListItem> GetProjects()
        {
            using (ORPEntities ctx = new ORPEntities())
            {
                var model = (from projects in ctx.ProjectMasters
                             select new SelectListItem
                             {
                                 Value = projects.ProjectID.ToString(),
                                 Text = projects.ProjectCode
                             }).ToList();
                return model;
            }
        }

        private TimeSheetReportModel GetTimeSheets(int timesheetId)
        {
            TimeSheetReportModel showReports = new TimeSheetReportModel();
            using (ORPEntities db = new ORPEntities())
            {
                showReports.TimeSheetTaskDetails = (from taskDetail in db.TaskDetails
                                                    join timesheet in db.TimeSheetDetails on taskDetail.TimeSheetID equals timesheet.TimeSheetID
                                                    join project in db.ProjectMasters on timesheet.ProjectID equals project.ProjectID
                                                    join user in db.InternalUsers on timesheet.UserID equals user.UserId
                                                    where taskDetail.TimeSheetID==timesheetId
                                                select new TimeSheetTaskDetails
                                                {
                                                    TaskId=taskDetail.TaskID,
                                                    TaskName = taskDetail.Name,
                                                    ProjectCode=project.ProjectCode,
                                                    Username=user.FirstName+" "+user.LastName,
                                                    StartTime = taskDetail.StartTime.ToString(),
                                                    EndTime = taskDetail.EndTime.ToString(),  
                                                }).OrderBy(m => m.TaskId).ToList();
                
            }
            return showReports;
        }

        private List<string> SplitWithInterval(DateTime fromTime, DateTime toTime,DateTime lunchStartTime, DateTime lunchEndTime, int interval)
        {
            List<string> breakIntervals = new List<string>();

            while (fromTime < toTime)
            {
                DateTime timeInterval1, timeInterval2;
                timeInterval1 = fromTime;

                fromTime = (fromTime >= lunchStartTime && fromTime <= lunchEndTime) ? fromTime.AddMinutes(60) : fromTime.AddMinutes(interval);
                timeInterval2 = fromTime;

                breakIntervals.Add((fromTime < toTime) ? (timeInterval1.ToShortTimeString() + "-" + timeInterval2.ToShortTimeString())
                        : (timeInterval1.ToShortTimeString() + "-" + toTime.ToShortTimeString()));
            }
            return breakIntervals;
        }

    }
}