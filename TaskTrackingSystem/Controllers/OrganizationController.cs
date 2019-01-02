using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models.OrganizationViewModels;

namespace SampleWebApp.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganization _organization;
        private readonly IUser _user;
        public OrganizationController(IOrganization organization, IUser user)
        {
            _organization = organization;
            _user = user;
        }
        // GET: Organization
        public ActionResult View(int id)
        {
            var data = _organization.GetById(id);
            var model = new OrganizationListDetailModel()
            {
                Id = data.Id,
                OrganizationName = data.Name,
                OrganizationType = data.OrganizationType.Name,
                Users = data.OrganizationUsers.Select(i => i.FullName).ToList(),
                Manager = _user.GetOrganizationManager(data.Id),
                Status = data.Status.Status
            };

            return View(model);
        }
        

    }
}