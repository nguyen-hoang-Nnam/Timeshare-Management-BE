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
    }
}
