using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel
{
    public enum TbTaskStatus: int
    {
        New = 0,
        InProgress,
        Completed
    }
}
