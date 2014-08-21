using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel.ViewModels
{
    public sealed class UserProjectVm
    {
        public string UserName { get; set; }
        public long ProjectId { get; set; }
    }
}
