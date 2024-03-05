using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.Models.Models;

namespace TimeshareManagement.DataAccess.Repository.IRepository
{
    public interface IBookingRequestRepository : IRepository<BookingRequest>
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendBookingConfirmationAsync(BookingRequest bookingRequest);
    }
}
