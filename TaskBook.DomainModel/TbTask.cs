using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBook.DomainModel
{
    [Table("Tasks")]
    public sealed class TbTask: Entity
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required]
        [MaxLength(512)]
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
