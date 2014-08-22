using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessReader
{
    internal sealed class SpNames
    {
        internal const string spGetProjectsAndManagers = "spGetProjectsAndManagers";
        internal const string spGetUserDetailsByUserName = "spGetUserDetailsByUserName";
        internal const string spGetPermissionsByRole = "spGetPermissionsByRole";
        internal const string spGetTasks = "spGetTasks";
        internal const string spGetTask = "spGetTask";
        internal const string spGetUsersByProjectId = "spGetUsersByProjectId";
        internal const string spGetUsersWithRolesByProjectId = "spGetUsersWithRolesByProjectId";
    }
}
