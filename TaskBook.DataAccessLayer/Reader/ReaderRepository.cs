using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DataAccessLayer.Reader
{
    public sealed class ReaderRepository: IReaderRepository, IDisposable
    {
        private readonly TaskBookDbContext _database;
        private readonly TbDataReader _dataReader;

        public ReaderRepository(TaskBookDbContext database)
        {
            _database = database;
            _dataReader = new TbDataReader(database);
        }

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

        public IQueryable<TaskUserVm> GetUserTasksByUserName(string userName)
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

        public IQueryable<TbUserVm> GetDeletedUsers()
        {
            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetDeletedUsers);
            return reader.Select(Projections.TbUserVmFromReader).AsQueryable();
        }

        public void Dispose()
        {
            _dataReader.Dispose();
        }
    }
}
