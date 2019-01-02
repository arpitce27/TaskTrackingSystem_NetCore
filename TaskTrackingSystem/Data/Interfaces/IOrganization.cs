using SampleWebApp.Data;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Data.Interfaces
{
    public interface IOrganization
    {
        void AddOrganization(Organization newOrg);
        IEnumerable<Organization> GetAll();
        Organization GetById(int orgId);
        IEnumerable<ApplicationUser> GetAllUsers(int orgId);
        void SaveChanges();
    }
}
