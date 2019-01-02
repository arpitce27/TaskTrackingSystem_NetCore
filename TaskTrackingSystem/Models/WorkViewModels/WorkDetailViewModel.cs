using SampleWebApp.Models.CommentViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.WorkViewModels
{
    public class WorkDetailViewModel
    {
        public WorkDetailViewModel()
        {
            Comments = new List<Comment>();
            Assignments = new List<WorkAssignment>();
            CommentCreate = new CommentCreateViewModel()
            {
                Id = this.Id
            };
        }
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public WorkType WorkType { get; set; }
        [Required]
        public WorkPriority Priority { get; set; }
        [Required]
        public WorkStatus Status { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }
        public string OrganizationName { get; set; }
        public List<Comment> Comments { get; set; }
        public List<WorkAssignment> Assignments { get; set; }
        public CommentCreateViewModel CommentCreate { get; set; }

    }
}
