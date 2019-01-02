using SampleWebApp.Data;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Services
{
    public class OrganizationStatusService : IOrganizationStatus
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrganization _organization;
        public OrganizationStatusService(ApplicationDbContext context, IOrganization organization)
        {
            _context = context;
            _organization = organization;
        }
        public OrganizationStatus GetByName(string statusName)
        {
            return _context.OrganizationStatuses.FirstOrDefault(status => status.Status == statusName);
        }

        public void SetStatusTo(int orgId, string statusName)
        {
            var org = _organization.GetById(orgId);
            org.Status = GetByName(statusName);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
