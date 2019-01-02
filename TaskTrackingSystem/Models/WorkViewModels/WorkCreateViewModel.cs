using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.WorkViewModels
{
    public class WorkCreateViewModel
    {
        public WorkCreateViewModel()
        {
            AssignedUsers = new List<AssignedUser>();
        }
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Work Type")]
        public WorkType WorkType { get; set; }

        [Required]
        public WorkPriority Priority { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Required]
        [Display(Name = "User Assignment")]
        public List<AssignedUser> AssignedUsers { get; set; }
    }

    public class AssignedUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public bool IsAssigned { get; set; }
    }
}
