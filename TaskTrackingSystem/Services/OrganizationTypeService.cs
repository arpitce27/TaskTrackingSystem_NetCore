using SampleWebApp.Data;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Services
{
    public class OrganizationTypeService : IOrganizationType
    {
        private readonly ApplicationDbContext _context;
        public OrganizationTypeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(OrganizationType newOrgType)
        {
            _context.Add(newOrgType);
            _context.SaveChanges();
        }

        public IEnumerable<OrganizationType> GetAll()
        {
            return _context.OrganizationTypes;
        }

        public OrganizationType GetByName(string typename)
        {
            return GetAll().FirstOrDefault(i => i.Name == typename);
        }
    }
}
