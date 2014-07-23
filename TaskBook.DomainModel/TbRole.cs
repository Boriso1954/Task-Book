using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskBook.DomainModel
{
    public class TbRole: IdentityRole
    {
        [Required]
        [MaxLength(256, ErrorMessage = "Description should be up to 256 characters long.")]
        public string Description { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
