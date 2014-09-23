using System.ComponentModel.DataAnnotations;

namespace TaskBook.DomainModel
{
    public sealed class ProjectUsers: Entity
    {
        [Required]
        public string UserId { get; set; }

        public TbUser User { get; set; }

        [Required]
        public long ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
