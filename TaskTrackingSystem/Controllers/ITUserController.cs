using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models.OrganizationViewModels;

namespace SampleWebApp.Controllers
{
    [Authorize(Roles = "ITUser")]
    public class ITUserController : Controller
    {
        private readonly IUser _user;
        public ITUserController(IUser user)
        {
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
    }
}