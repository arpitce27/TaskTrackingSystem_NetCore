using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApp.Data
{
    public static class ApplicationDbInitializer
    {
        public static async Task InitilizeAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            //context.Database.EnsureCreated();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            string[] roles = new string[] { "Administrator", "Manager", "ITUser" };
            // Adding Administrator role if it is not available
            foreach (var r in roles)
            {
                var roleCheck = await roleManager.RoleExistsAsync(r);
                if (!roleCheck)
                {
                    //create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(r));
                }
            }

            // Adding organization type if it is not available - OrganizationType
            string[] orgTypes = new string[] { "Administrative", "ITOrganization" };
            foreach (var ot in orgTypes)
            {
                var org = context.OrganizationTypes.Where(i => i.Name == ot).Any();
                if (!org)
                {
                    context.Add(new OrganizationType() { Name = ot, Description = ot });
                    context.SaveChanges();
                }
            }

            // Adding Organization Statuses
            string[] Orgstatus = new string[] { "Active", "Pending", "Deactivated" };
            foreach (var os in Orgstatus)
            {
                var status = context.OrganizationStatuses.Where(i => i.Status == os).Any();
                if (!status)
                {
                    context.Add(new OrganizationStatus() { Status = os, Description = os + "Organization" });
                    context.SaveChanges();
                }
            }

            // Creating Super Admin - This application owner account
            var anyAdminOrg = context.Organizations.Where(i => i.OrganizationType.Name == "Administrative").Any();
            if (!anyAdminOrg)
            {
                context.Add(new Organization() {
                    Name = "TMS Administrator",
                    OrganizationType = context.OrganizationTypes.FirstOrDefault(name => name.Name == "Administrative"),
                    Status = context.OrganizationStatuses.FirstOrDefault(st => st.Status == "Active")
                });
                context.SaveChanges();
            }

            // Adding User into application owner account
            var anyAdmin = userManager.Users.Where(user => user.UserName == "Administrator").Any();
            if (!anyAdmin)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    MyOrganization = context.Organizations.FirstOrDefault(i => i.OrganizationType.Name == "Administrative")
                };

                var result = await userManager.CreateAsync(user, "Admin_123");
                if (result.Succeeded)
                {
                    ApplicationUser admuser = new ApplicationUser();
                    admuser = await userManager.FindByEmailAsync("admin@admin.com");
                    await userManager.AddToRoleAsync(admuser, "Administrator");
                }
                context.SaveChanges();
            }

            string[] workTypes = new string[] { "Task", "Event", "Project" };
            // Adding Work Type
            foreach (var wt in workTypes)
            {
                var TowrkTypeCheck = context.WorkType.Where(i => i.Type == wt).Any();
                if (!TowrkTypeCheck)
                {
                    context.WorkType.Add(new WorkType()
                    {
                        Type = wt
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
