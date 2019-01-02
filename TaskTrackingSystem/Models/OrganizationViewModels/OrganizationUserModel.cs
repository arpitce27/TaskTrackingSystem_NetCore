using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.OrganizationViewModels
{
    public class OrganizationUserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string OrganizationName { get; set; }
    }
    public class OrganizationUserListModel
    {
        public IEnumerable<OrganizationUserModel> Users { get; set; }
    }
}
