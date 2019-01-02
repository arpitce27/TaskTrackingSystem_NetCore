using Microsoft.AspNetCore.Identity;
using SampleWebApp.Models;
using SampleWebApp.Models.OrganizationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Data.Interfaces
{
    public interface IUser
    {
        ApplicationUser GetCurrentUser();
        string GetCurrentUserRole();
        bool IsInRole(ApplicationUser user, string rolename);
        bool IsCurrentAuthenticated();
        string GetOrganizationManager(int orgId);
        int GetMyOrganizationId();

        string GetUserRole(string userId);
        IdentityResult AddUserToMyOrganization(ApplicationUser user, string password);
        IEnumerable<ApplicationUser> GetITUser();
        ApplicationUser GetById(string uID);
        void SaveChanges();

    }
}
