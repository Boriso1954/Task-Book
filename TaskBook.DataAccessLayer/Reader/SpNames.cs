using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessLayer.Reader
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
        internal const string spGetUserTasks = "spGetUserTasks";
        internal const string spGetDeletedUsers = "spGetDeletedUsers";
        internal const string spGetDeletedProjects = "spGetDeletedProjects";
        internal const string spGetTasksByUserName = "spGetTasksByUserName";
    }
}
