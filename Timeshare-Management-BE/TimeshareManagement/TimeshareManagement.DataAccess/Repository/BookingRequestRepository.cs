using AutoMapper;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace TimeshareManagement.DataAccess.Repository
{
    public class BookingRequestRepository : Repository<BookingRequest>, IBookingRequestRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public BookingRequestRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task SendBookingConfirmationAsync(BookingRequest bookingRequest)
        {
            if (bookingRequest.User != null && !string.IsNullOrEmpty(bookingRequest.User.Email))
            {
                var subject = "Booking Confirmation";
                var body = $"Dear {bookingRequest.User.UserName},\n\nYour booking for room {bookingRequest.timeshareId} on {bookingRequest.bookingDate} is confirmed.";

                await SendEmailAsync(bookingRequest.User.Email, subject, body);
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "h0angnnam1123@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("h0angnnam1123@gmail.com", "rhuo ggwe iofg lqqv");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        public async Task<IEnumerable<BookingRequest>> GetByUserId(string userId)
        {
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            return await _db.BookingRequests
                .Where(t => t.User.Id == userId)
                .ToListAsync();
        }
        public async Task<BookingRequest> GetByIdAsync(int id)
        {
            return await _db.BookingRequests.FindAsync(id);
        }
        public async Task<IEnumerable<BookingRequest>> GetByStatusId(int statusId)
        {
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            return await _db.BookingRequests
                .Where(t => t.TimeshareStatus.timeshareStatusId == statusId)
                .ToListAsync();
        }
        public async Task<IEnumerable<object>> GetByTimeshareIdAndStatusId(int timeshareId, int statusId)
        {
            return await _db.BookingRequests
                .Include(b => b.Timeshare) 
                .Include(b => b.TimeshareStatus) 
                .Include(b => b.User)
                .Where(b => b.timeshareId == timeshareId && b.timeshareStatusId == statusId
                            && b.Timeshare != null && b.TimeshareStatus != null && b.User != null)
                .Select(b => new
                {
                    b.bookingRequestId,
                    b.bookingDate,
                    /*b.timeshareId,*/
                    TimeshareInfo = new
                    {
                        
                        b.Timeshare.timeshareId,
                    },
                    UserInfo = new
                    {
                        b.User.Id,
                        b.User.UserName,
                        b.User.Name,
                        b.User.Email,
                        b.User.PhoneNumber
                    },
                    b.timeshareStatusId
                })
                    .ToListAsync();
        }
        public async Task<int> GetBookingCountByTimeshareId(int timeshareId)
        {
            return await _db.BookingRequests.CountAsync(b => b.timeshareId == timeshareId);
        }
    }
}
