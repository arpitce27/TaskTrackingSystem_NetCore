using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleWebApp.Data;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using SampleWebApp.Models.OrganizationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Services
{
    public class UserService : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganization _organization;
        private readonly ApplicationDbContext _context;
        public UserService(UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor httpContextAccessor, 
            SignInManager<ApplicationUser> signInManager,
            IOrganization organization,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _organization = organization;
            _context = context;
        }

        public ApplicationUser GetCurrentUser()
        {
            return _context.Users.Include(o => o.MyOrganization).FirstOrDefault(i => i.Id == GetCurrentUserId());
        }

        public string GetCurrentUserRole()
        {
            return _userManager.GetRolesAsync(GetCurrentUser()).Result.First();
        }

        public int GetMyOrganizationId()
        {
            return GetCurrentUser().MyOrganization.Id;
        }

        public string GetOrganizationManager(int orgId)
        {
            var users = _organization.GetAllUsers(orgId);

            var userName = users.Any(i => IsInRole(i, "Manager") == true)? 
                           users.FirstOrDefault(i => IsInRole(i, "Manager") == true).FullName :
                           users.Any(i => IsInRole(i, "Administrator") == true)? 
                               users.FirstOrDefault(i => IsInRole(i, "Administrator") == true).FullName : 
                               "Not Found" ;

            return userName;
        }

        public bool IsCurrentAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(ApplicationUser user, string rolename)
        {
            return _userManager.IsInRoleAsync(user, rolename).Result;
        }

        private string GetCurrentUserId()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result.Id;
        }
        public string GetUserRole(string userId)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(i => i.Id == userId);
            var a = _userManager.GetRolesAsync(user).Result;
            return _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        }

        public IdentityResult AddUserToMyOrganization(ApplicationUser user, string password)
        {
            var result = _userManager.CreateAsync(user, password);
            if (result.Result.Succeeded)
            {
                var res = _userManager.AddToRoleAsync(user, "ITUser");
                if (res.Result.Succeeded)
                    SaveChanges();
                else
                    return res.Result;
            }
            return result.Result;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetITUser()
        {
            return _organization.GetAllUsers(GetMyOrganizationId()).Where(i => IsInRole(i, "ITUser") == true);
        }

        public ApplicationUser GetById(string uId)
        {
            return _userManager.Users.FirstOrDefault(i => i.Id == uId);
        }
    }
}
