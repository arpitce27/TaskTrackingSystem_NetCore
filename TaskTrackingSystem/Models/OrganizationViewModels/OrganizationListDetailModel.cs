using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.OrganizationViewModels
{
    public class OrganizationListDetailModel
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationType { get; set; }
        public string Manager { get; set; }
        public string Status { get; set; }
        public IEnumerable<string> Users { get; set; }
    }
}
