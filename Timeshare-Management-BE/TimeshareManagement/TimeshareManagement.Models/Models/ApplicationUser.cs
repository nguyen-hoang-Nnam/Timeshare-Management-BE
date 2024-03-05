using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        /*public string LastName { get; set; }*/
        /*public ICollection<Timeshare>? timeshares { get; set; }*/
    }
}
