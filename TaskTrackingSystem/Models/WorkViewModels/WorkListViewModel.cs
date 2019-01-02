using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.WorkViewModels
{
    public class WorkListViewModel
    {
        public WorkListViewModel()
        {
            Works = new List<WorkListDetailViewModel>();
            WorkTypes = new List<WorkType>();
        }
        public IEnumerable<WorkListDetailViewModel> Works { get; set; }
        public List<WorkType> WorkTypes { get; set; }
    }
}
