using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class RoomAmenities
    {
        [Key]
        public int roomAmenitiesId { get; set; }
        public string roomAmenitiesName { get; set;}
        /*public int? roomDetailId { get; set; }
        [ForeignKey("roomDetailId")]
        public RoomDetail? RoomDetail { get; set; }*/
    }
}
