using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Data.Interfaces
{
    public interface IWorkType
    {
        List<WorkType> GetAll();
        WorkType GetByName(string type);
        WorkType GetById(int workTypeID);
    }
}
