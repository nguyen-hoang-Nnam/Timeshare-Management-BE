using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class TimeshareStatus
    {
        [Key]
        public int timeshareStatusId { get; set; }
        public string? timeshareStatusName { get; set; }
    }
}
