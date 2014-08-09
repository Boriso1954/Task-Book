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

        // TODO Obsolete
        internal const string spGetUserPermissionsByUserName = "spGetUserPermissionsByUserName";
        internal const string spGetPermissionsByRole = "spGetPermissionsByRole";
    }
}
