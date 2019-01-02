using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using SampleWebApp.Models.AccountViewModels;
using SampleWebApp.Models.OrganizationViewModels;

namespace SampleWebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IOrganization _organization;
        private readonly IUser _user;
        public ManagerController(IOrganization organization, IUser user)
        {
            _organization = organization;
            _user = user;
        }
        public ActionResult Index()
        {
            var user = _user.GetCurrentUser();
            var model = new OrganizationUserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                OrganizationName = user.MyOrganization.Name
            };

            return View(model);
        }
        public ActionResult MyUsers()
        {
            int orgId = _user.GetMyOrganizationId();
            var users = _organization.GetAllUsers(orgId).Select(us => new OrganizationUserModel()
            {
                Id = us.Id,
                FirstName = us.FirstName,
                LastName = us.LastName,
                Email = us.Email,
                UserName = us.UserName,
                UserRole = _user.GetUserRole(us.Id),
                OrganizationName = us.MyOrganization.Name
            }).ToList();

            var model = new OrganizationUserListModel()
            {
                Users = users
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult AddUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(UserRegistrationViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MyOrganization = _organization.GetById(_user.GetMyOrganizationId()),
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = _user.AddUserToMyOrganization(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyUsers");
                }
                AddErrors(result);
            }
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}