using Microsoft.EntityFrameworkCore;
using SampleWebApp.Data;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Services
{
    public class WorkService : IWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrganization _organization;
        private readonly IUser _user;
        public WorkService(ApplicationDbContext context, IOrganization organization, IUser user)
        {
            _context = context;
            _organization = organization;
            _user = user;
        }
        public IEnumerable<Work> GetAllWorks()
        {
            return _context.Work
                .Include(i => i.AssignedUsers)
                .Include(i => i.Comments)
                .Include(i => i.Type)
                .Include(i => i.Organization);
        }
        public void AddWork(Work newWork)
        {
            _context.Add(newWork);
            Savechanges();
        }
        public Work GetById(int workId)
        {
            return GetAllWorks().FirstOrDefault(i => i.Id == workId);
        }
        public IEnumerable<WorkAssignment> CurrentAssignments(int workId)
        {
            return _context.WorkAssignment
                .Include(i => i.User)
                .Include(i => i.Work)
                .Where(i => i.Work.Id == workId);
        }

        public IEnumerable<Work> GetOrganizationWork(string workType = "", WorkStatus? status = null, string shortOn = "", string shortType = "Asc")
        {
            int myOrgId = _user.GetMyOrganizationId();
            var allWork = GetAllWorks().Where(i => i.Organization.Id == myOrgId);

            if (workType != null && !workType.Equals(""))
            {
                allWork = allWork.Where(i => i.Type.Type == workType);
            }
            else if (status != null)
            {
                allWork = allWork.Where(i => i.Status == status);
            }

            if(!string.IsNullOrEmpty(shortOn) || !shortOn.Equals(""))
            {
                if (shortType.Equals("Desc"))
                {
                    switch (shortOn)
                    {
                        case "Priority":
                            allWork = allWork.OrderByDescending(i => i.Priority);
                            break;
                        case "Status":
                            allWork = allWork.OrderByDescending(i => i.Status);
                            break;
                        case "CreatedOn":
                            allWork = allWork.OrderByDescending(i => i.CreatedOn);
                            break;
                        case "Deadline":
                            allWork = allWork.OrderByDescending(i => i.Deadline);
                            break;
                        default: break;
                    }
                }
                else
                {
                    switch (shortOn)
                    {
                        case "Priority":
                            allWork = allWork.OrderBy(i => i.Priority);
                            break;
                        case "Status":
                            allWork = allWork.OrderBy(i => i.Status);
                            break;
                        case "CreatedOn":
                            allWork = allWork.OrderBy(i => i.CreatedOn);
                            break;
                        case "Deadline":
                            allWork = allWork.OrderBy(i => i.Deadline);
                            break;
                        default: break;
                    }
                }
            }

            return allWork;
        }
        public IEnumerable<Work> GetUserWork(string workType = "", WorkStatus? status = null, string shortOn = "", string shortType = "")
        {
            int myOrgId = _user.GetMyOrganizationId();
            string myUserId = _user.GetCurrentUser().Id;
            IEnumerable<Work> allWork = new List<Work>();
            if (workType != null && !workType.Equals(""))
            {
                var workIds = _context.WorkAssignment
                    .Where(i => i.User.Id == myUserId & i.Work.Organization.Id == myOrgId & i.Work.Type.Type == workType)
                    .Select(j => j.Work.Id).ToList();

                allWork = _context.Work
                    .Include(i => i.Type)
                    .Where(i => workIds.Contains(i.Id));
            }
            else if (status != null)
            {
                var workIds = _context.WorkAssignment
                    .Where(i => i.User.Id == myUserId & i.Work.Organization.Id == myOrgId & i.Work.Status == status)
                    .Select(j => j.Work.Id).ToList();

                allWork = _context.Work
                    .Include(i => i.Type)
                    .Where(i => workIds.Contains(i.Id));
            }
            else
            {
                var workIds = _context.WorkAssignment
                    .Where(i => i.User.Id == myUserId & i.Work.Organization.Id == myOrgId)
                    .Select(j => j.Work.Id).ToList();

                allWork = _context.Work
                    .Include(i => i.Type)
                    .Where(i => workIds.Contains(i.Id));
            }

            if (!string.IsNullOrEmpty(shortOn) || !shortOn.Equals(""))
            {
                if (shortType.Equals("Desc"))
                {
                    switch (shortOn)
                    {
                        case "Priority":
                            allWork = allWork.OrderByDescending(i => i.Priority);
                            break;
                        case "Status":
                            allWork = allWork.OrderByDescending(i => i.Status);
                            break;
                        case "CreatedOn":
                            allWork = allWork.OrderByDescending(i => i.CreatedOn);
                            break;
                        case "Deadline":
                            allWork = allWork.OrderByDescending(i => i.Deadline);
                            break;
                        default: break;
                    }
                }
                else
                {
                    switch (shortOn)
                    {
                        case "Priority":
                            allWork = allWork.OrderBy(i => i.Priority);
                            break;
                        case "Status":
                            allWork = allWork.OrderBy(i => i.Status);
                            break;
                        case "CreatedOn":
                            allWork = allWork.OrderBy(i => i.CreatedOn);
                            break;
                        case "Deadline":
                            allWork = allWork.OrderBy(i => i.Deadline);
                            break;
                        default: break;
                    }
                }
            }

            return allWork;
        }

        public void UpdateAssignment(int workId, List<WorkAssignment> newAssignments)
        {
            var currentAssignments = CurrentAssignments(workId);
            if (newAssignments.Count > 0)
            {
                foreach (var assignment in currentAssignments)
                {
                    if (!newAssignments.Select(i => i.User.Id == assignment.User.Id & i.Work.Id == assignment.Work.Id).Any())
                    {
                        _context.WorkAssignment.Remove(assignment);
                    }
                }

                currentAssignments = CurrentAssignments(workId);
                var time = DateTime.Now;
                foreach (var newAssignment in newAssignments)
                {
                    if (!currentAssignments.Where(i => i.User.Id == newAssignment.User.Id & i.Work.Id == newAssignment.Work.Id).Any())
                    {
                        newAssignment.AssignedDate = time;
                        _context.WorkAssignment.Add(newAssignment);
                    }
                }
                Savechanges();
            }
        }

        public WorkAssignment GetByUserWork(int workId, string userId)
        {
            return CurrentAssignments(workId).FirstOrDefault(i => i.User.Id == userId);
        }

        public void UpdateWork(Work oldWork)
        {
            var newWork = GetById(oldWork.Id);
            if (newWork != null)
            {
                newWork.Title = oldWork.Title;
                newWork.Description = oldWork.Description;
                newWork.Priority = oldWork.Priority;
                newWork.Status = oldWork.Status;
                newWork.Deadline = oldWork.Deadline;

                newWork.Type = oldWork.Type;
                UpdateAssignment(oldWork.Id, oldWork.AssignedUsers.ToList());

                Savechanges();
            }
        }

        #region WorkStatus
        public void SetWorkStatus(int workId, WorkStatus newStatus)
        {
            var work = GetById(workId);
            work.Status = newStatus;
            Savechanges();
        }
        #endregion

        #region WorkLog
        public void AddWorkLog(WorkLog workLog)
        {
            _context.Add(workLog);
            Savechanges();
        }

        public List<WorkLog> AllWorkLogs(int workId)
        {
            var a = _context.WorkLogs
                .Include(i => i.Work)
                .Include(i => i.User)
                .Where(i => i.Work.Id == workId);
            return _context.WorkLogs
                .Include(i => i.Work)
                .Include(i => i.User)
                .Where(i => i.Work.Id == workId).ToList();
        }

        public List<WorkLog> WorkLogsByUser(int workId, string userId)
        {
            return _context.WorkLogs
                .Where(i => i.User.Id == userId && i.Work.Id == workId).ToList();
        }

        public double TotalHoursWorked(int workId)
        {
            return AllWorkLogs(workId).Select(i => i.TimeSpent).Sum();
        }

        public double TotalHoursWorkedByUser(int workId, string userId)
        {
            return _context.WorkLogs
                .Where(i => i.User.Id == userId && i.Work.Id == workId)
                .Select(i => i.TimeSpent).Sum();
        }
        #endregion

        public void Savechanges()
        {
            _context.SaveChanges();
        }

        public void AddComment(Comment newComment)
        {
            _context.Add(newComment);
            Savechanges();
        }

        public List<Comment> GetCommentsById(int workId)
        {
            return _context.Comments.Where(i => i.Work.Id == workId).ToList();
        }
    }
}
