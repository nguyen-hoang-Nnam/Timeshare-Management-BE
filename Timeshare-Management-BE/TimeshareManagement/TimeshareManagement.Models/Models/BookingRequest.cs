using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
/*using Newtonsoft.Json;*/
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class BookingRequest
    {
        [Key]
        public int bookingRequestId {  get; set; }
        public DateTime? bookingDate { get; set; }
        public int? timeshareId { get; set; }
        [ForeignKey("timeshareId")]
        [JsonIgnore]
        public Timeshare? Timeshare { get; set; }
        public string? Id { get; set; }
        [ForeignKey("Id")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int? timeshareStatusId { get; set; }
        [ForeignKey("timeshareStatusId")]
        [JsonIgnore]
        public TimeshareStatus? TimeshareStatus { get; set; }

    }
}
