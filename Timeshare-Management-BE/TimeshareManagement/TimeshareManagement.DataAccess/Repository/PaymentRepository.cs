using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;

namespace TimeshareManagement.DataAccess.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public PaymentRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Payment>> GetByUserId(string userId)
        {
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            return await _db.Payments
                .Where(t => t.BookingRequest.User.Id == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Payment>> GetPaymentByBookingId(int bookingId)
        {
            var payments = await _db.Payments
                .Include(p => p.BookingRequest)
                    .ThenInclude(b => b.User) // Include ApplicationUser
                .Include(p => p.BookingRequest)
                    .ThenInclude(b => b.Timeshare) // Include Timeshare
                .Where(p => p.BookingRequestId == bookingId)
                .ToListAsync();

            // Convert the anonymous type to Payment type
            return payments.Select(p => new Payment
            {
                PaymentId = p.PaymentId,
                PaymentDate = p.PaymentDate,
                Expiration = p.Expiration,
                CardNumber = p.CardNumber,
                Amount = p.Amount,
                CVC = p.CVC,
                BookingRequestId = p.BookingRequestId,
                timeshareStatusId = p.timeshareStatusId,
                BookingRequest = p.BookingRequest != null ? new BookingRequest
                {
                    bookingRequestId = p.BookingRequest.bookingRequestId,
                    bookingDate = p.BookingRequest.bookingDate,
                    User = p.BookingRequest.User != null ? new ApplicationUser
                    {
                        Id = p.BookingRequest.User.Id,
                        Name = p.BookingRequest.User.Name,
                        Email = p.BookingRequest.User.Email,
                    } : null,
                    Timeshare = p.BookingRequest.Timeshare != null ? new Timeshare
                    {
                        timeshareId = p.BookingRequest.Timeshare.timeshareId,
                        timeshareName = p.BookingRequest.Timeshare.timeshareName,
                    } : null
                } : null
            });
        }
        /*public async Task<IEnumerable<Payment>> GetPaymentByBookingId(int bookingId)
        {
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            var payments = await _db.Payments
                .Include(p => p.BookingRequest)
                    .ThenInclude(b => b.User) // Include ApplicationUser
                .Include(p => p.BookingRequest)
                    .ThenInclude(b => b.Timeshare) // Include Timeshare
                .Where(p => p.BookingRequestId == bookingId)
                .ToListAsync();

            // Convert the anonymous type to Payment type
            return payments.Select(p => new Payment
            {
                PaymentId = p.PaymentId,
                PaymentDate = p.PaymentDate,
                Expiration = p.Expiration,
                CardNumber = p.CardNumber,
                CVC = p.CVC,
                BookingRequestId = p.BookingRequestId,
                BookingRequest = new BookingRequest
                {
                    bookingRequestId = p.BookingRequest.bookingRequestId,
                    bookingDate = p.BookingRequest.bookingDate,
                    User = new ApplicationUser
                    {
                        Id = p.BookingRequest.User.Id,
                        Name = p.BookingRequest.User.Name,
                        Email = p.BookingRequest.User.Email,
                    },
                    Timeshare = new Timeshare
                    {
                        timeshareId = p.BookingRequest.Timeshare.timeshareId,
                        timeshareName = p.BookingRequest.Timeshare.timeshareName,
                        // Add other properties of Timeshare as needed
                    }
                }
            });
        }*/
    }
}
