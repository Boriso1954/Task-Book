using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessReader.ViewModels;

namespace TaskBook.DataAccessReader
{
    public sealed class ReaderRepository: IDisposable
    {
        private readonly TbDataReader _dataReader;

        public ReaderRepository(TbDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        public IEnumerable<ProjectManagerVm> GetProjectsAndManagers()
        {
            var reader = _dataReader.ExecuteReader(CommandType.StoredProcedure, SpNames.spGetProjectsAndManagers);
            return reader.Select(Projections.ProjectManagerVmFromReader);
        }

        public void Dispose()
        {
            _dataReader.Dispose();
        }
    }
}
