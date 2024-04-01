using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public string PaymentName { get; set; }
        public string timeshareName { get; set; }
        public string userEmail { get; set; }
        public DateTime paymentDate { get; set; }
        public int Amount { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVC { get; set; }
        public int bookingRequestId { get; set; }
        public int timeshareStatusId { get; set; }
    }
}
