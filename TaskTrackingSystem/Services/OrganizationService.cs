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
    public class OrganizationService : IOrganization
    {
        private readonly ApplicationDbContext _context;
        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddOrganization(Organization newOrg)
        {
            _context.Add(newOrg);
        }

        public IEnumerable<Organization> GetAll()
        {
            return _context.Organizations
                .Include(us => us.OrganizationUsers)
                .Include(os => os.Status)
                .Include(tp => tp.OrganizationType);
        }
        public Organization GetById(int orgId)
        {
            return GetAll().FirstOrDefault(us => us.Id == orgId);
        }
        public IEnumerable<ApplicationUser> GetAllUsers(int orgId)
        {
            return GetById(orgId).OrganizationUsers;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
