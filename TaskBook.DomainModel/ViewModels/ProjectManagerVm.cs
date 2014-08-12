using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel.ViewModels
{
    public sealed class ProjectManagerVm
    {
        public long ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
    }
}
