using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskBook.DomainModel
{
    public sealed class TbRole: IdentityRole
    {
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
