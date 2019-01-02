using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SampleWebApp.Data;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using SampleWebApp.Models.CommentViewModels;
using SampleWebApp.Models.WorkLogViewModels;
using SampleWebApp.Models.WorkViewModels;

namespace SampleWebApp.Controllers
{
    [Authorize(Roles = "Manager, ITUser")]
    public class WorkController : Controller
    {
        private readonly IWork _work;
        private readonly IWorkType _workType;
        private readonly IUser _user;
        private readonly IOrganization _organization;
        private readonly string currentRole;
        public WorkController(IWork work, IWorkType workType, IUser user, IOrganization organization)
        {
            _work = work;
            _workType = workType;
            _user = user;
            _organization = organization;
            currentRole = _user.GetCurrentUserRole();
        }

        public ActionResult AllWork(string workType = "", WorkStatus? status = null, string shortBy = "")
        {
            var model = new WorkListViewModel();
            ViewBag.UserRole = currentRole.ToString();
            ViewBag.workType = workType.ToString();
            ViewBag.PriorityShort = "Asc";
            ViewBag.Status = "Asc";
            ViewBag.CreatedOn = "Asc";
            ViewBag.Deadline = "Asc";
            string shortOn = "", shortType = "";
            if (shortBy.Contains("PriorityShort"))
            {
                shortOn = "Priority";
                if (shortBy.Contains("Desc"))
                {
                    shortType = "Asc";
                    ViewBag.PriorityShort = "Asc";
                }
                else
                {
                    shortType = "Desc";
                    ViewBag.PriorityShort = "Desc";
                }                 
            }
            else if (shortBy.Contains("Status"))
            {
                shortOn = "Status";
                if (shortBy.Contains("Desc"))
                {
                    shortType = "Asc";
                    ViewBag.Status = "Asc";
                }
                else
                {
                    shortType = "Desc";
                    ViewBag.Status = "Desc";
                }
            }
            else if (shortBy.Contains("CreatedOn"))
            {
                shortOn = "CreatedOn";
                if (shortBy.Contains("Desc"))
                {
                    shortType = "Asc";
                    ViewBag.CreatedOn = "Asc";
                }
                else
                {
                    shortType = "Desc";
                    ViewBag.CreatedOn = "Desc";
                }
            }
            else if (shortBy.Contains("Deadline"))
            {
                shortOn = "Deadline";
                if (shortBy.Contains("Desc"))
                {
                    shortType = "Asc";
                    ViewBag.Deadline = "Asc";
                }
                else
                {
                    shortType = "Desc";
                    ViewBag.Deadline = "Desc";
                }
            }

            if (currentRole.Equals("Manager"))
            {
                model.Works = _work.GetOrganizationWork(workType, status, shortOn, shortType).Select(i => new WorkListDetailViewModel()
                {
                    Id = i.Id,
                    Title = i.Title,
                    WorkType = i.Type,
                    Priority = i.Priority,
                    Status = i.Status,
                    CreatedOn = i.CreatedOn,
                    Deadline = i.Deadline,
                    AssignedUsers = _work.CurrentAssignments(i.Id).Select(j => j.User.FullName).ToList()
                });
            }
            else if (currentRole.Equals("ITUser"))
            {
                model.Works = _work.GetUserWork(workType, status, shortOn, shortType).Select(i => new WorkListDetailViewModel()
                {
                    Id = i.Id,
                    Title = i.Title,
                    WorkType = i.Type,
                    Priority = i.Priority,
                    Status = i.Status,
                    CreatedOn = i.CreatedOn,
                    Deadline = i.Deadline,
                    AssignedUsers = _work.CurrentAssignments(i.Id).Select(j => j.User.FullName).ToList()
                });
            }
            model.WorkTypes.AddRange(model.Works.Select(i => i.WorkType).Distinct().ToList());
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            List<WorkType> workType = new List<WorkType>();
            workType.AddRange(_workType.GetAll());
            ViewBag.WorkTypes = workType;

            ViewBag.Priority = Enum.GetNames(typeof(WorkPriority)).ToList().Select(i => new
            {
                Text = i,
                Value = i
            });

            var model = new WorkCreateViewModel() {
                Deadline = DateTime.Now,
                AssignedUsers = _user.GetITUser().Select(i => new AssignedUser() {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    IsAssigned = false
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkCreateViewModel model)
        {
            model.AssignedUsers = model.AssignedUsers.Where(i => i.IsAssigned == true).ToList();
            int id;
            if (int.TryParse(Request.Form["WorkType"].ToString(), out id))
            {
                model.WorkType = _workType.GetById(id);
            }
            try
            {
                var wAssignment = new List<WorkAssignment>();
                DateTime now = DateTime.Now;
                Work data = new Work() {
                    Title = model.Title,
                    Description = model.Description,
                    Priority = model.Priority,
                    Status = model.AssignedUsers.Count() > 0 ? WorkStatus.Assigned : WorkStatus.NotAssigned,
                    CreatedOn = now,
                    Deadline = model.Deadline,
                    Organization = _organization.GetById(_user.GetMyOrganizationId()),
                    Type = model.WorkType
                };
                _work.AddWork(data);

                foreach (var u in model.AssignedUsers)
                {
                    wAssignment.Add(new WorkAssignment()
                    {
                        User = _user.GetById(u.Id),
                        Work = data,
                        AssignedDate = now
                    });
                }
                _work.UpdateAssignment(data.Id, wAssignment);
                return RedirectToAction("AllWork");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            List<WorkType> workType = new List<WorkType>();
            workType.AddRange(_workType.GetAll());
            ViewBag.WorkTypes = workType;
            
            ViewBag.Priority = Enum.GetNames(typeof(WorkPriority)).ToList().Select(i => new
            {
                Text = i,
                Value = i
            });

            var data = _work.GetById(id);
            var currentAssignedUserIds = _work.CurrentAssignments(data.Id).Select(i => i.User.Id);
            var model = new WorkCreateViewModel()
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                WorkType = data.Type,
                Priority = data.Priority,
                Deadline = data.Deadline,
                AssignedUsers = _user.GetITUser().Select(i => new AssignedUser()
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    IsAssigned = currentAssignedUserIds.Contains(i.Id) ? true : false
                }).ToList()
            };
            ViewBag.SelectedWorkType = model.WorkType.Id;
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, WorkCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            model.AssignedUsers = model.AssignedUsers.Where(i => i.IsAssigned == true).ToList();
            int WorkTypeId;
            if (int.TryParse(Request.Form["WorkType"].ToString(), out WorkTypeId))
            {
                model.WorkType = _workType.GetById(WorkTypeId);
            }

            try
            {
                var wAssignment = new List<WorkAssignment>();
                DateTime now = DateTime.Now;
                Work data = new Work()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Priority = model.Priority,
                    Status = model.AssignedUsers.Count() > 0 ? WorkStatus.Assigned : WorkStatus.NotAssigned,
                    CreatedOn = now,
                    Deadline = model.Deadline,
                    Organization = _organization.GetById(_user.GetMyOrganizationId()),
                    Type = model.WorkType
                };

                foreach (var u in model.AssignedUsers)
                {
                    wAssignment.Add(new WorkAssignment()
                    {
                        User = _user.GetById(u.Id),
                        Work = data,
                        AssignedDate = now
                    });
                }
                data.AssignedUsers = wAssignment;
                _work.UpdateWork(data);
                return RedirectToAction("AllWork");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

        }

        public ActionResult Detail(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.UserRole = currentRole.ToString();
            var data = _work.GetById(id);

            if (currentRole.Equals("ITUser") && !data.Status.Equals(WorkStatus.Inprogress))
            {
                _work.SetWorkStatus(data.Id, WorkStatus.Inprogress);
            }

            var model = new WorkDetailViewModel()
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                WorkType = data.Type,
                Priority = data.Priority,
                Status = data.Status,
                CreatedOn = data.CreatedOn,
                Deadline = data.Deadline,
                Comments = data.Comments.ToList(),
                Assignments = _work.CurrentAssignments(data.Id).ToList(),
                OrganizationName = data.Organization.Name
            };

            return View(model);
        }

        //// GET: Work/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var workDetailViewModel = await _context.WorkDetailViewModel
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (workDetailViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(workDetailViewModel);
        //}

        //// POST: Work/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var workDetailViewModel = await _context.WorkDetailViewModel.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.WorkDetailViewModel.Remove(workDetailViewModel);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool WorkDetailViewModelExists(int id)
        //{
        //    return _context.WorkDetailViewModel.Any(e => e.Id == id);
        //}

        [HttpGet]
        [Authorize(Roles = "ITUser")]
        public async Task<IActionResult> AddWorkLog(int id)
        {
            if (id == 0)
                return NotFound();
            var work = _work.GetById(id);
            //var previousWorkLogs = _work.WorkLogsByUser(id, _user.GetCurrentUser().Id);
            //double alredySpent = previousWorkLogs.Count() > 0 ? previousWorkLogs.Select(i => i.TimeSpent).Sum() : 0;
            int days = (int)(work.Deadline - DateTime.Now).TotalDays;
            string dedlineText = days >= 0 ? days == 0 ? "Due Today" : days == 1 ? "Due Tomorrow" : "Due in " + days + " days" : "Overdue by " + Math.Abs(days) + " days";

            var model = new WorkLogCreateViewModel()
            {
                Id = id,
                work = work,
                LogDate = DateTime.Now,
                DeadLine = dedlineText//,
                //AlreadySpent = alredySpent,
                //PreviousWorkLogs = previousWorkLogs
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorkLog(int id, WorkLogCreateViewModel model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError(string.Empty, "Please try some times later!");
                return View(model);
            }
            var work = _work.GetById(id);


            if (model.LogDate > DateTime.Now || (int)(model.EndTime - model.StartTime).Hours < 0)
            {
                if(model.LogDate > DateTime.Now)
                    ModelState.AddModelError(string.Empty, "You can not enter future date!");
                if((int)(model.EndTime - model.StartTime).Hours < 0)
                    ModelState.AddModelError(string.Empty, "Please enter the correct timings!");

                List<WorkLog> previousWorkLogs = _work.WorkLogsByUser(id, _user.GetCurrentUser().Id);
                double alredySpent = previousWorkLogs.Count() > 0 ? previousWorkLogs.Select(i => i.TimeSpent).Sum() : 0;
                int days = (int)(work.Deadline - DateTime.Now).TotalDays;
                string dedlineText = days >= 0 ? days == 0 ? "Due Today" : days == 1 ? "Due Tomorrow" : "Due in " + days + " days" : "Overdue by " + Math.Abs(days) + " days";

                model.work = work;
                model.LogDate = DateTime.Now;
                model.DeadLine = dedlineText;
                model.AlreadySpent = alredySpent;
                model.PreviousWorkLogs = previousWorkLogs;

                return View(model);
            }


            if (currentRole.Equals("ITUser") && !work.Status.Equals(WorkStatus.Inprogress))
            {
                _work.SetWorkStatus(work.Id, WorkStatus.Inprogress);
            }
            _work.AddWorkLog(new WorkLog()
            {
                CreatedDate = DateTime.Now,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                User = _user.GetCurrentUser(),
                Work = work
            });

            return RedirectToAction("AllWork");
        }

        [HttpGet]
        public ActionResult AddComment(int id)
        {
            if (id == 0)
                return null;
            var model = new CommentCreateViewModel()
            {
                Id = id
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int id, CommentCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var workDetail = _work.GetById(model.Id);
            if (currentRole.Equals("ITUser") && !workDetail.Status.Equals(WorkStatus.Inprogress))
            {
                _work.SetWorkStatus(workDetail.Id, WorkStatus.Inprogress);
            }
            Comment newComment = new Comment()
            {
                PostTime = DateTime.Now,
                Content = model.Content,
                Work = workDetail,
                User = _user.GetCurrentUser()
            };

            try
            {
                _work.AddComment(newComment);
                return RedirectToAction("Detail", new { Id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        public ActionResult MarkCompleted(int id)
        {
            _work.SetWorkStatus(id, WorkStatus.Completed);
            return RedirectToAction("AllWork");
        }
    }
}
