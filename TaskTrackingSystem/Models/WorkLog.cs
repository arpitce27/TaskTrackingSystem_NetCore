using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models
{
    public class WorkLog
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        public double TimeSpent
        {
            get { return (int)(EndTime - StartTime).TotalHours; }
            set { this.TimeSpent = (int)(EndTime - StartTime).TotalHours; }
        }

        public virtual Work Work { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
