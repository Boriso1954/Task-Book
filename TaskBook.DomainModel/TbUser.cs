using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskBook.DomainModel
{
    public sealed class TbUser: IdentityUser
    {
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? DeletedDate { get; set; }

        public ICollection<ProjectUsers> Projects { get; set; }

    }
}
