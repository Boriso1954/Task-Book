using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel
{
    public class ProjectUsers: Entity
    {
        [Required]
        public string UserId { get; set; }
        public TbUser User { get; set; }
        [Required]
        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
