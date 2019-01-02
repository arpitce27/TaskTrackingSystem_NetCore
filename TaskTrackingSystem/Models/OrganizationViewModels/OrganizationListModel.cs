using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.OrganizationViewModels
{
    public class OrganizationListModel
    {
        public IEnumerable<OrganizationListDetailModel> Organizations { get; set; }
    }
}
