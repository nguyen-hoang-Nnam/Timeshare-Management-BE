using AutoMapper;
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
    public class TimeshareDetailRepository : Repository<TimeshareDetail>, ITimeshareDetailRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public TimeshareDetailRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
        {
            _db = db;
            _mapper = mapper;
        }
    }
}
