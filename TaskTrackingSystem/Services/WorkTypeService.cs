using SampleWebApp.Data;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Services
{
    public class WorkTypeService : IWorkType
    {
        private readonly ApplicationDbContext _context;
        public WorkTypeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<WorkType> GetAll()
        {
            return _context.WorkType.ToList();
        }

        public WorkType GetById(int workTypeId)
        {
            return GetAll().FirstOrDefault(i => i.Id == workTypeId);
        }

        public WorkType GetByName(string type)
        {
            return GetAll().FirstOrDefault(i => i.Type.Equals(type));
        }
    }
}
