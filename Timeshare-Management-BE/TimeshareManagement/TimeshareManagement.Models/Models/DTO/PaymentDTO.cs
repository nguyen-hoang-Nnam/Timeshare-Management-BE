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
        public DateTime paymentDate { get; set; }
        public int bookingRequestId { get; set; }
    }
}
