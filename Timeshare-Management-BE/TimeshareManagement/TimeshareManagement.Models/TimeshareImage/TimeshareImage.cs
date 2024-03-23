using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.Models.Models;

namespace TimeshareManagement.Models.TimeshareImage
{
    public class TimeshareImage
    {
        public Timeshare Timeshare { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
