using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.WorkViewModels
{
    public class WorkListDetailViewModel
    {
        public WorkListDetailViewModel()
        {
            AssignedUsers = new List<string>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public WorkType WorkType { get; set; }
        public WorkPriority Priority { get; set; }
        public WorkStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }
        public List<string> AssignedUsers { get; set; }
    }
}
