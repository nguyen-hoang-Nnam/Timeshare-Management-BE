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
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }
        [Required(ErrorMessage = "Payment Amount is required.")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$", ErrorMessage = "Invalid expiration date format. Use MM/YY or MM/YYYY.")]
        public string Expiration { get; set; }
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVC must be a 3 or 4-digit number.")]
        public string CVC { get; set; }

        public int? BookingRequestId { get; set; }

        [ForeignKey("BookingRequestId")]
        [JsonIgnore]
        public BookingRequest? BookingRequest { get; set; }
        public int? timeshareStatusId { get; set; }
        [ForeignKey("timeshareStatusId")]
        [JsonIgnore]
        public TimeshareStatus? TimeshareStatus { get; set; }

    }
}
