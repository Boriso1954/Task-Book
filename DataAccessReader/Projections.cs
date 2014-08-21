using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DataAccessReader
{
    internal static class Projections
    {
        internal static Func<SqlDataReader, ProjectManagerVm> ProjectManagerVmFromReader = (r) => new ProjectManagerVm()
        {
            ProjectId = (long)r["ProjectId"],
            ProjectTitle = (string)r["Title"],
            ManagerId = (string)r["UserId"],
            ManagerName = (string)r["UserName"]
        };

        internal static Func<SqlDataReader, TbUserVm> TbUserVmFromReader = (r) => new TbUserVm()
        {
            UserId = (string)r["UserId"],
            UserName = (string)r["UserName"],
            Email = r["Email"] != DBNull.Value ? (string)r["Email"] : string.Empty,
            FirstName = (string)r["FirstName"],
            LastName = (string)r["LastName"],
            Role = (string)r["Role"],
            ProjectId = r["ProjectId"] != DBNull.Value ? (long?)r["ProjectId"] : null,
            ProjectTitle = r["ProjectTitle"] != DBNull.Value ? (string)r["ProjectTitle"] : "N/A"
        };

        internal static Func<SqlDataReader, PermissionVm> PermissionVmFromReader = (r) => new PermissionVm()
        {
            Name = (string)r["Name"],
            Description = (string)r["Description"]
        };

        internal static Func<SqlDataReader, TaskVm> TaskVmFromReader = (r) => new TaskVm()
        {
            TaskId = (long)r["TaskId"],
            ProjectId = (long)r["ProjectId"],
            Title = (string)r["Title"],
            Description = (string)r["Description"],
            CreatedDate = (DateTimeOffset)r["CreatedDate"],
            DueDate = (DateTimeOffset)r["DueDate"],
            Status = GetStringTaskStatus((int)r["Status"]),
            CreatedBy = (string)r["CreatedBy"],
            AssignedTo = (string)r["AssignedTo"],
            CompletedDate = r["CompletedDate"] != DBNull.Value ? (DateTimeOffset?)r["CompletedDate"] : null
        };

        internal static Func<SqlDataReader, UserProjectVm> UserProjectVmFromReader = (r) => new UserProjectVm()
        {
            UserName = (string)r["UserName"],
            ProjectId = (long)r["ProjectId"]
        };

        private static string GetStringTaskStatus(int taskStatus)
        {
            string status = string.Empty;
            switch(taskStatus)
            {
                case 0:
                    status = "New";
                    break;
                case 1:
                    status = "In Progress";
                    break;
                case 2:
                    status = "Completed";
                    break;
            }
            return status;
        }
    }
}
