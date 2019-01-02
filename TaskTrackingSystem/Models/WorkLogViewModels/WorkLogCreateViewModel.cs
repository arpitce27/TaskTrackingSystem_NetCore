using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.WorkLogViewModels
{
    public class WorkLogCreateViewModel
    {
        public int Id { get; set; }
        public Work work { get; set; }
        public double AlreadySpent { get; set; }
        public string DeadLine { get; set; }

        [Requried]
        [Display(Name = "Log Date")]
        public DateTime LogDate { get; set; }

        [Requried]
        [Display(Name = "Starting Time")]
        public DateTime StartTime { get; set; }

        [Requried]
        [Display(Name = "Ending Time")]
        public DateTime EndTime { get; set; }
        public List<WorkLog> PreviousWorkLogs { get; set; }

    }
}
