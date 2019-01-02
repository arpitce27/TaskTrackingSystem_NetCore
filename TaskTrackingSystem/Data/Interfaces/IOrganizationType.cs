using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Data.Interfaces
{
    public interface IOrganizationType
    {
        IEnumerable<OrganizationType> GetAll();
        void Add(OrganizationType newOrgType);
        OrganizationType GetByName(string typename);
    }
}
