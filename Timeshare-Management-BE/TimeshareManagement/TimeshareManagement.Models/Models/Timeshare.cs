using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class Timeshare
    {
        [Key]
        public int timeshareId { get; set; }
        public string? timeshareName { get; set; }
        public string? Image { get; set; }
        public int Price { get; set; }
        public string? Address { get; set; }
        public string? Detail {  get; set; }
        /*public DateTime ExpirationDate { get; set; }*/
        public DateTime PublicDate { get; set; }
        public int? timeshareStatusId { get; set; }
        [ForeignKey("timeshareStatusId")]
        [JsonIgnore]
        public TimeshareStatus? TimeshareStatus { get; set; }
        public int? placeId { get; set; }
        [ForeignKey("placeId")]
        [JsonIgnore]
        public Place? Place { get; set; }
        public int? confirmTimeshare {  get; set; }
        public string? Id { get; set; }
        [ForeignKey("Id")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }

    }
}
