using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models
{
    public class WorkAssignment
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Work Work { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }
    }
}
