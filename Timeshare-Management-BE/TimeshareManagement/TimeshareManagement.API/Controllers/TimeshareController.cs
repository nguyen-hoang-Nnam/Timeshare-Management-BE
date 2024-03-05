using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;
using TimeshareManagement.Models.Role;

namespace TimeshareManagement.API.Controllers
{
    [Route("api/timeshare")]
    [ApiController]
    public class TimeshareController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ITimeshareRepository _timeshareRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly ITimeshareStatusRepository _timeshareStatusRepository;

        public TimeshareController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, ITimeshareRepository timeshareRepository, IUserRepository userRepository, IPlaceRepository placeRepository, ITimeshareStatusRepository timeshareStatusRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _timeshareRepository = timeshareRepository;
            _userRepository = userRepository;
            _placeRepository = placeRepository;
            _timeshareStatusRepository = timeshareStatusRepository;
        }
        [HttpGet]
        [Route("GetAllTimeshare")]
        public async Task<IActionResult> GetAllTimeshare()
        {
            try
            {
                var timeshare = await _timeshareRepository.GetAll();
                return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Timeshare retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetTimeshareById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var timeshare = await _timeshareRepository.GetById(id);
                if (timeshare == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found." });
                } else
                {
                    return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Timeshare retrieved successfully." });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("CreateTimeshare")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> CreateTimeshare([FromBody] Timeshare timeshare)
        {
            try
            {
                if (timeshare == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare object is null." });
                }
                timeshare.confirmTimeshare = 0;
                IEnumerable<Place> places = _placeRepository.GetAllItem();
                if (timeshare.Place != null)
                {
                    timeshare.Place = places.FirstOrDefault(p => p.placeName == timeshare.Place.placeName);
                } else
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare object is null." });
                }
                IEnumerable<TimeshareStatus> status = _timeshareStatusRepository.GetAllItem();
                if (timeshare.TimeshareStatus != null)
                {
                    timeshare.TimeshareStatus = status.FirstOrDefault(s => s.timeshareStatusName == timeshare.TimeshareStatus.timeshareStatusName);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare object is null." });
                }

                await _timeshareRepository.Create(timeshare);
                return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Create Timeshare successfully. Awaiting confirmation." });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPut]
        [Route("UpdateTimeshare/{id:int}")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> UpdateTimeshare(int id, [FromBody] Timeshare timeshare)
        {
            try
            {
                var existingTimeshare = await _timeshareRepository.GetById(id);
                if (existingTimeshare == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found." });
                } else
                {
                    existingTimeshare.timeshareName = timeshare.timeshareName;
                    existingTimeshare.Price = timeshare.Price;
                    existingTimeshare.Address = timeshare.Address;
                    existingTimeshare.Image = timeshare.Image;
                    existingTimeshare.Detail = timeshare.Detail;
                    existingTimeshare.placeId = timeshare.placeId;
                    /*existingTimeshare.Id = timeshare.Id;*/
                    existingTimeshare.timeshareStatusId = timeshare.timeshareStatusId;
                    //
                    await _timeshareRepository.Update(existingTimeshare);
                }
                return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Update Timeshare successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete]
        [Route("DeleteTimeshare/{id:int}")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> DeleteTimeshare(int id)
        {
            try
            {
                await _timeshareRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete Timeshare successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTimeshare(int page, int pageSize, decimal? searchPrice)
        {
            try
            {
                Expression<Func<Timeshare, bool>> filter = null;

                /*if (!string.IsNullOrEmpty(searchName))
                {
                    filter = entity => entity.Name.Contains(searchName);
                }*/
                /*if (searchPrice.HasValue)
                {
                    filter = entity => entity.Price == searchPrice.Value;
                }*/
                var item = await _timeshareRepository.GetPagedAsync(page, pageSize, filter);
                return Ok(new ResponseDTO { Result = item, IsSucceed = true, Message = "Paging Room successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("ConfirmTimeshare")]
        public async Task<IActionResult> ConfirmTimeshare(int timeshareId)
        {
            try
            {
                Timeshare timeshare = await _timeshareRepository.GetById(timeshareId);
                if (timeshare == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found"});
                }
                timeshare.confirmTimeshare = 1;
                await _timeshareRepository.Update(timeshare);
                return Ok(new ResponseDTO { Result = timeshare, IsSucceed = false, Message = "Timeshare confirmed successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            } 
        }
    }
}

