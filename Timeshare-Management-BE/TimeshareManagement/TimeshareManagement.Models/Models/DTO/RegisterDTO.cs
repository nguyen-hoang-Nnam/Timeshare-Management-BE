using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.Models.Validation;

namespace TimeshareManagement.Models.Models.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /*[Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }*/

        [Required(ErrorMessage = "UserName is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        /*public string? Role { get; set; }*/
        [Required(ErrorMessage = "PhoneNumber is required")]
        [PhoneNumber(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
