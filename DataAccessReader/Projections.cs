using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessReader.ViewModels;

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
            Email = (string)r["Email"],
            FirstName = (string)r["FirstName"],
            LastName = (string)r["LastName"],
            ProjectId = r["ProjectId"] != DBNull.Value ? (long?)r["ProjectId"] : null,
            ProjectTitle = r["ProjectTitle"] != DBNull.Value ? (string)r["ProjectTitle"] : "N/A"
        };
    }
}
