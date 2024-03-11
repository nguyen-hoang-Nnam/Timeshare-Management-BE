using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.DataAccess.Repository.IRepository
{
    public interface ITimeshareRepository : IRepository<Timeshare>
    {
        Task<Timeshare> GetByIdAsync(int id);
        Task<IEnumerable<Timeshare>> GetByUserId(string userId);
        Task DeleteById(int id);
        /*Task<Timeshare> GetById(int id);*/
        Task Update(Timeshare timeshare);
        Task<IEnumerable<Timeshare>> GetByStatusId(int statusId);
        Task<List<Timeshare>> GetAllAsync();
    }
}
