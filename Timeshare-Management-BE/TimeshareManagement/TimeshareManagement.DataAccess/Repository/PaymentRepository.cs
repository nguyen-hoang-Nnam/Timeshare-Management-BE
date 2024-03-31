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
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            return await _db.Payments
                .Where(t => t.BookingRequest.bookingRequestId == bookingId)
                .ToListAsync();
        }
    }
}
