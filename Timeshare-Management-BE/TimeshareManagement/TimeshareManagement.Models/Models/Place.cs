using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class Place
    {
        [Key]
        public int placeId { get; set; }
        public string? placeName { get; set; }
    }
}
