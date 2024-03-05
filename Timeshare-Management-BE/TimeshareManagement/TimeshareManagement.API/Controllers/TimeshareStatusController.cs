using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeshareManagement.Models.Models.DTO;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Role;
using AutoMapper;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository.IRepository;

namespace TimeshareManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeshareStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ITimeshareStatusRepository _timeshareStatusRepository;
        public TimeshareStatusController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, ITimeshareStatusRepository timeshareStatusRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _timeshareStatusRepository = timeshareStatusRepository;
        }
        [HttpGet]
        [Route("GetAllTimeshareStatus")]
        public async Task<IActionResult> GetAllTimeshareStatus()
        {
            try
            {
                var timeshareStatus = await _timeshareStatusRepository.GetAll();
                return Ok(new ResponseDTO { Result = timeshareStatus, IsSucceed = true, Message = "Timeshare status retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetTimeshareStatusById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var timeshareStatus = await _timeshareStatusRepository.GetById(id);
                if (timeshareStatus == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare status not found." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = timeshareStatus, IsSucceed = true, Message = "Timeshare status retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("CreateTimeshareStatus")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> CreateTimeshareStatus([FromBody] TimeshareStatus timeshareStatus)
        {
            try
            {
                await _timeshareStatusRepository.Create(timeshareStatus);
                return Ok(new ResponseDTO { Result = timeshareStatus, IsSucceed = true, Message = "Create Timeshare Status successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPut]
        [Route("UpdateTimeshareStatus/{id:int}")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> UpdateTimeshareStatus(int id, [FromBody] TimeshareStatus timeshareStatus)
        {
            try
            {
                var existingTimeshareStatus = await _timeshareStatusRepository.GetById(id);
                if (existingTimeshareStatus == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare status not found." });
                }
                else
                {
                    existingTimeshareStatus.timeshareStatusName = timeshareStatus.timeshareStatusName;
                    //
                    await _timeshareStatusRepository.Update(existingTimeshareStatus);
                }
                return Ok(new ResponseDTO { Result = timeshareStatus, IsSucceed = true, Message = "Update Timeshare status successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete]
        [Route("DeleteTimeshareStatus/{id:int}")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> DeleteTimeshareStatus(int id)
        {
            try
            {
                await _timeshareStatusRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete Timeshare status successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
