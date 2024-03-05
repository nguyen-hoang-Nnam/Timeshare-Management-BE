using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Migrations;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.API.Controllers
{
    [Route("api/timeshareDetail")]
    [ApiController]
    public class TimeshareDetailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ITimeshareDetailRepository _timeshareDetailRepository;
        private readonly ITimeshareRepository _timeshareRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserRepository _userRepository;

        public TimeshareDetailController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, ITimeshareDetailRepository timeshareDetailRepository, ITimeshareRepository timeshareRepository, IPlaceRepository placeRepository, IUserRepository userRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _timeshareDetailRepository = timeshareDetailRepository;
            _timeshareRepository = timeshareRepository;
            _placeRepository = placeRepository;
            _userRepository = userRepository;
        }
        [HttpGet]
        [Route("GetAllTimeshareDetail")]
        public async Task<IActionResult> GetAllTimeshareDetail()
        {
            try
            {
                var timeshareDetail = await _timeshareDetailRepository.GetAll();
                return Ok(new ResponseDTO { Result = timeshareDetail, IsSucceed = true, Message = "Timeshare Detail retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetTimeshareDetailById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var timeshareDetail = await _timeshareDetailRepository.GetById(id);
                if (timeshareDetail == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare Detail not found." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = timeshareDetail, IsSucceed = true, Message = "Timeshare Detail retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("CreateTimeshareDetail")]
        public async Task<IActionResult> CreateTimeshareDetail([FromBody] TimeshareDetail timeshareDetail)
        {
            try
            {
                /*if (timeshareDetail == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare Detail object is null." });
                }

                IEnumerable<Timeshare> timeshares = _timeshareRepository.GetAllItem();
                if (timeshareDetail.Timeshare != null)
                {
                    timeshareDetail.Timeshare = timeshares.FirstOrDefault(ts => ts.timeshareName == timeshareDetail.Timeshare.timeshareName);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare Detail object is null." });
                }*/

                await _timeshareDetailRepository.Create(timeshareDetail);
                return Ok(new ResponseDTO { Result = timeshareDetail, IsSucceed = true, Message = "Create Timeshare Detail successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPut]
        [Route("UpdateTimeshareDetail/{id:int}")]
        public async Task<IActionResult> UpdateTimeshareDetail(int id, [FromBody] TimeshareDetail timeshareDetail)
        {
            try
            {
                var existingTimeshareDetail = await _timeshareDetailRepository.GetById(id);
                if (existingTimeshareDetail == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare Detail not found." });
                }
                else
                {
                    existingTimeshareDetail.Image = timeshareDetail.Image;
                    existingTimeshareDetail.Detail = timeshareDetail.Detail;
                    /*existingTimeshareDetail.timeshareId = timeshareDetail.timeshareId;*/
                   
                    await _timeshareDetailRepository.Update(timeshareDetail);
                }
                return Ok(new ResponseDTO { Result = timeshareDetail, IsSucceed = true, Message = "Update Timeshare Detail successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete]
        [Route("DeleteTimeshareDetail/{id:int}")]
        public async Task<IActionResult> DeleteTimeshareDetail(int id)
        {
            try
            {
                await _timeshareDetailRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete Timeshare Detail successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
