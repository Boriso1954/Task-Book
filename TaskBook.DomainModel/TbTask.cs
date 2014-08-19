using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel
{
    [Table("Tasks")]
    public class TbTask: Entity
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(64, ErrorMessage = "Title should be from 1 to 64 characters long.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(512, ErrorMessage = "Description should be from 1 to 512 characters long.")]
        public string Description { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedById { get; set; }
        public TbUser CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset DueDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? CompletedDate { get; set; }

        public TbTaskStatus Status { get; set; }

        public string AssignedToId { get; set; }
        public TbUser AssignedTo { get; set; }
    }
}
