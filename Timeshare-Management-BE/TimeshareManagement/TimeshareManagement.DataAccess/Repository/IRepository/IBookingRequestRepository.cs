﻿using System;
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
        Task<IEnumerable<BookingRequest>> GetByUserId(string userId);
        Task<BookingRequest> GetByIdAsync(int id);
        Task<IEnumerable<BookingRequest>> GetByStatusId(int statusId);
        Task<IEnumerable<object>> GetByTimeshareIdAndStatusId(int timeshareId, int statusId);
        Task<IEnumerable<object>> GetBookingSuccess(int timeshareId, int statusId);
        Task<BookingRequest> GetById(int id);
    }
}
