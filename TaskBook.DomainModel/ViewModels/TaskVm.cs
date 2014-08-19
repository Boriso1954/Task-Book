using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel.ViewModels
{
    public sealed class TaskVm
    {
        public long TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? CompletedDate { get; set; }
    }
}
