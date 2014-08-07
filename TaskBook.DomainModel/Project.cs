using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel
{
    public class Project: Entity
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(32, ErrorMessage = "Title should be from 1 to 32 characters long.")]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? DeletedDate { get; set; }

        public ICollection<TbUser> Users { get; set; }
    }
}