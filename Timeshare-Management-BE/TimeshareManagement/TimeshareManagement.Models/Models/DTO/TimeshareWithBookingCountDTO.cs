using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models.DTO
{
    public class TimeshareWithBookingCountDTO
    {
        public int TimeshareId { get; set; }
        public string? TimeshareName { get; set; }
        public int BookingCount { get; set; }
    }
}
