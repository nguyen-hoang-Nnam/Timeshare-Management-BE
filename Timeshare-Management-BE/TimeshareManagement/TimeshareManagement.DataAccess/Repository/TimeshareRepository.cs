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
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.DataAccess.Repository
{
    public class TimeshareRepository : Repository<Timeshare>, ITimeshareRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public TimeshareRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Timeshare> GetByIdAsync(int id)
        {
            return await _db.Timeshares.FindAsync(id);
        }
        public async Task<IEnumerable<Timeshare>> GetByUserId(string userId)
        {
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            return await _db.Timeshares
                .Where(t => t.User.Id == userId)
                .ToListAsync();
        }

        public async Task DeleteById(int id)
        {
            var timeshare = await _db.Timeshares.FindAsync(id);

            if (timeshare != null)
            {
                // Manually delete related BookingRequests entities
                var relatedBookingRequests = await _db.BookingRequests
                    .Where(br => br.timeshareId == id)
                    .ToListAsync();

                _db.BookingRequests.RemoveRange(relatedBookingRequests);

                // Delete the Timeshare
                _db.Timeshares.Remove(timeshare);

                await _db.SaveChangesAsync();
            }
        }
        /*public async Task<Timeshare> GetById(int id)
        {
            return await _dbSet
                .Include(t => t.TimeshareStatus)
                .FirstOrDefaultAsync(t => t.timeshareId == id);
        }*/
        public async Task Update(Timeshare timeshare)
        {
            _db.Timeshares.Update(timeshare);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Timeshare>> GetByStatusId(int statusId)
        {
            // Assuming there's a property named ApplicationUserId in the Timeshare model
            return await _db.Timeshares
                .Where(t => t.TimeshareStatus.timeshareStatusId == statusId)
                .ToListAsync();
        }
        public async Task<List<Timeshare>> GetAllAsync()
        {
            return await _db.Timeshares.ToListAsync();
        }
    }
}
