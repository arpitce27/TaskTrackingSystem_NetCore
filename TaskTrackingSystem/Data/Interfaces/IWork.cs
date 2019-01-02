using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Data.Interfaces
{
    public interface IWork
    {
        void AddWork(Work newWork);
        void UpdateWork(Work work);
        Work GetById(int workId);
        IEnumerable<Work> GetOrganizationWork(string type = "", WorkStatus? status = null, string shortOn = "", string shortType = "");
        IEnumerable<Work> GetUserWork(string type = "", WorkStatus? status = null, string shortOn = "", string shortType = "");
        IEnumerable<WorkAssignment> CurrentAssignments(int workId);

        void UpdateAssignment(int workId, List<WorkAssignment> assignedUsers);
        WorkAssignment GetByUserWork(int workId, string userId);


        void SetWorkStatus(int workId, WorkStatus newStatus);

        #region WorkLog

        void AddWorkLog(WorkLog workLog);
        List<WorkLog> AllWorkLogs(int workId);
        List<WorkLog> WorkLogsByUser(int workId, string userId);
        double TotalHoursWorked(int workId);
        double TotalHoursWorkedByUser(int workId, string userId);

        #endregion

        #region WorkComments

        void AddComment(Comment newComment);
        List<Comment> GetCommentsById(int workId);

        #endregion


        void Savechanges();
    }
}
