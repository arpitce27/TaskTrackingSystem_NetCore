using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.OrganizationViewModels
{
    public class OrganizationDetailModel: OrganizationListDetailModel
    {
        public IEnumerable<OrganizationUserModel> UsersDetail { get; set; }

    }
}
