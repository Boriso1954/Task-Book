using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskBook.DomainModel
{
    public class TbUser: IdentityUser
    {
        [Required]
        [MaxLength(25, ErrorMessage = "FirstName shall be from 1 to 25 characters long.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "LastName shall be from 1 to 25 characters long.")]
        public string LastName { get; set; }
    }
}
