using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskBook.DomainModel
{
    public sealed class Permission: Entity
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [MaxLength(128)]
        public string Description { get; set; }

        public ICollection<TbRole> Roles { get; set; }
    }
}
