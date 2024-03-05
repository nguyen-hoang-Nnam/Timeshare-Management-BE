using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class TimeshareDetail
    {
        [Key]
        public int timeshareDetailId { get; set; }
        public string? Image { get; set; }
        public string? Detail { get; set; }
        
        /*public int? timeshareId { get; set; }
        [ForeignKey("timeshareId")]
        public Timeshare? Timeshare { get; set; }*/
    }
}
