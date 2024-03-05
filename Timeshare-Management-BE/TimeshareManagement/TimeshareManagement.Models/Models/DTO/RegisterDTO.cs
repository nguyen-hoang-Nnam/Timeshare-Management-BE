using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /*public string? Role { get; set; }*/
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }
    }
}
