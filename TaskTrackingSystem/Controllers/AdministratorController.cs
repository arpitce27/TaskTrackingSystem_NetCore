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
using SampleWebApp.Models.OrganizationViewModels;

namespace SampleWebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly IOrganization _organization;
        private readonly IUser _user;
        public AdministratorController(IOrganization organization, IUser user)
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


        public ActionResult Roles()
        {
            return View();
        }

        public ActionResult Organizations(string shortBy = "NA")
        {
            var data = _organization.GetAll();
            var orgs = new List<OrganizationListDetailModel>();
            foreach (var d in data)
            {
                var orgnization = new OrganizationListDetailModel()
                {
                    Id = d.Id,
                    OrganizationName = d.Name,
                    OrganizationType = d.OrganizationType.Name,
                    Users = d.OrganizationUsers.Select(i => i.FullName).ToList(),
                    Manager = _user.GetOrganizationManager(d.Id),
                    Status = d.Status.Status
                };
                orgs.Add(orgnization);
            }

            var model = new OrganizationListModel() {
                Organizations = orgs
            };

            return View(model);
        }

        public ActionResult OrganizationDetail(int id)
        {
            return View();
        }
    }
}