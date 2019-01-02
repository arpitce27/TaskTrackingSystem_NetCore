using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Data.Interfaces
{
    public interface IOrganizationStatus
    {
        OrganizationStatus GetByName(string statusName);
        void SetStatusTo(int orgId, string statusName);
        void SaveChanges();
    }
}
