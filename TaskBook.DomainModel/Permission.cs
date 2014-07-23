using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel
{
    public class Permission:Entity
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(16, ErrorMessage = "Name should be from 1 to 16 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(128, ErrorMessage = "Description should be from 1 to 128 characters long.")]
        public string Description { get; set; }
        public ICollection<TbRole> Roles { get; set; }

    }
}
