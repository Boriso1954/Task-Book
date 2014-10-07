using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DataAccessLayer.Reader
{
    /// <summary>
    /// Provides API for reading data from the database
    /// </summary>
    public sealed class ReaderRepository: IReaderRepository, IDisposable
    {
        private readonly TaskBookDbContext _database;
        private readonly TbDataReader _dataReader;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Database context</param>
        public ReaderRepository(TaskBookDbContext database)
        {
            _database = database;
            _dataReader = new TbDataReader(database);
        }

        /// <summary>
        /// Returns list of projects and project managers 
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>List of projects and managers</returns>
        /// <remarks>If project ID is null returns all the projects</remarks>
        public IQueryable<ProjectManagerVm> GetProjectsAndManagers(long? projectId = null)
        {
            SqlDataReader reader = null;

            if(projectId == null)
            {
                reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetProjectsAndManagers);
            }
            else
            {
                var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@projectId",
                        Value = projectId
                    }
                };
                reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetProjectsAndManagers, parameters);
            }
            return reader.Select(Projections.ProjectManagerVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns user's deatils by user name
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's details including role</returns>
        public IQueryable<TbUserRoleVm> GetUserByUserName(string userName)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@userName",
                        Value = userName
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetUserDetailsByUserName, parameters);
            return reader.Select(Projections.TbUserRoleVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns role permissions
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of the role permissions</returns>
        public IQueryable<PermissionVm> GetPermissionsByRole(string roleName)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@roleName",
                        Value = roleName
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetPermissionsByRole, parameters);
            return reader.Select(Projections.PermissionVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of projects and project managers
        /// </summary>
        /// <param name="managerName">Manager name</param>
        /// <returns>List of projects and managers</returns>
        public IQueryable<ProjectManagerVm> GetProjectByManagerName(string managerName)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@userName",
                        Value = managerName
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetProjectsAndManagers, parameters);
            return reader.Select(Projections.ProjectManagerVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of tasks
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>If project ID is null returns all the tasks</returns>
        public IQueryable<TaskVm> GetTasks(long? projectId = null)
        {
            SqlDataReader reader = null;

            if(projectId == null)
            {
                reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetTasks);
            }
            else
            {
                var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@projectId",
                        Value = projectId
                    }
                };
                reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetTasks, parameters);
            }
            return reader.Select(Projections.TaskVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns task details
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>List of tasks</returns>
        public IQueryable<TaskVm> GetTask(long id)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@id",
                        Value = id
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetTask, parameters);
            return reader.Select(Projections.TaskVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of users related to the specified project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of users</returns>
        public IQueryable<UserProjectVm> GetUsersByProjectId(long projectId)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@projectId",
                        Value = projectId
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetUsersByProjectId, parameters);
            return reader.Select(Projections.UserProjectVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of users including role related to specified project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of users with roles</returns>
        public IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@projectId",
                        Value = projectId
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetUsersWithRolesByProjectId, parameters);
            return reader.Select(Projections.TbUserRoleVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of user's tasks 
        /// </summary>
        /// <param name="userName">User nam</param>
        /// <returns>List of user's tasks</returns>
        public IQueryable<TaskVm> GetTasksByUserName(string userName)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@userName",
                        Value = userName
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetTasksByUserName, parameters);
            return reader.Select(Projections.TaskVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of user's tasks
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>List of user's tasks</returns>
        /// <remarks>In comparison with GetTasksByUserName this method returns less data</remarks>
        public IQueryable<TaskUserVm> GetUserTasks(string userName)
        {
            var parameters = new TbParameters()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@userName",
                        Value = userName
                    }
                };

            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetUserTasks, parameters);
            return reader.Select(Projections.TaskUserVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of soft deleted users (marked as deleted)
        /// </summary>
        /// <returns>List of soft deleted users</returns>
        public IQueryable<TbUserVm> GetDeletedUsers()
        {
            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetDeletedUsers);
            return reader.Select(Projections.TbUserVmFromReader).AsQueryable();
        }

        /// <summary>
        /// Returns list of soft deleted projects (marked as deleted)
        /// </summary>
        /// <returns>List of soft deleted projects</returns>
        public IQueryable<ProjectVm> GetDeletedProjects()
        {
            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetDeletedProjects);
            return reader.Select(Projections.ProjectVmFromReader).AsQueryable();
        }

        public void Dispose()
        {
            _dataReader.Dispose();
        }
    }
}
