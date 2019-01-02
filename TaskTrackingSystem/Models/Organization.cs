using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public OrganizationType OrganizationType { get; set; }

        [Required]
        public OrganizationStatus Status { get; set; }

        public virtual IEnumerable<ApplicationUser> OrganizationUsers { get; set; }
        public virtual IEnumerable<Work> OurWorks { get; set; }
    }
}
