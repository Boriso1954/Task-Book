using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskBook.WebApi.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The User name cannot exceed 256 characters.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The First name cannot exceed 25 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The Last name cannot exceed 25 characters.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "The password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}