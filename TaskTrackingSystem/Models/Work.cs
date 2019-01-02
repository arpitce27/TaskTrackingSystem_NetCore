using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models
{
    public class Work
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public WorkPriority Priority { get; set; }

        [Required]
        public WorkStatus Status { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual WorkType Type { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual ICollection<WorkAssignment> AssignedUsers { get; set; }
    }

    public enum WorkPriority
    {
        Low, Medium, High
    }

    public enum WorkStatus
    {
        NotAssigned, Assigned, Inprogress, Overdue, Completed
    }
}
