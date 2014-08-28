using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel.ViewModels
{
    public sealed class ProjectVm
    {
        public long ProjectId { get; set; }
        public string Title { get; set; }
    }
}
