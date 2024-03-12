using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Timeshare retrieved successfully." });
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
                timeshare.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 1};
                if (timeshare.User != null && timeshare.User.Id != null)
                {
                    timeshare.User = await _userRepository.GetByIdAsync(timeshare.User.Id);
                    if (timeshare.User == null)
                    {
                        return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "User not found." });
                    }
                }

                if (timeshare.Place != null && timeshare.Place.placeId != null)
                {
                    timeshare.Place = await _placeRepository.GetByIdAsync(timeshare.Place.placeId);
                    if (timeshare.Place == null)
                    {
                        return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Place not found." });
                    }
                }
                if (timeshare.TimeshareStatus != null && timeshare.TimeshareStatus.timeshareStatusId != null)
                {
                    timeshare.TimeshareStatus = await _timeshareStatusRepository.GetByIdAsync(timeshare.TimeshareStatus.timeshareStatusId);
                    if (timeshare.TimeshareStatus == null)
                    {
                        return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Status not found." });
                    }
                }
                /*timeshare.ExpirationDate = DateTime.Now;*/
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
                    existingTimeshare.Id = timeshare.Id;
                    existingTimeshare.timeshareStatusId = timeshare.timeshareStatusId;
                    existingTimeshare.PublicDate = timeshare.PublicDate;
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
        public async Task<IActionResult> GetTimeshare(int page, int pageSize = 5, decimal? searchPrice = null)
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
        [Route("ConfirmTimeshare/{timeshareId}")]
        public async Task<IActionResult> ConfirmTimeshare(int timeshareId)
        {
            try
            {
                Timeshare timeshare = await _timeshareRepository.GetById(timeshareId);
                if (timeshare == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found" });
                }
                    // Update timeshare status ID directly
                    timeshare.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 2 };
                    if (timeshare.TimeshareStatus != null && timeshare.TimeshareStatus.timeshareStatusId != null)
                    {
                        timeshare.TimeshareStatus = await _timeshareStatusRepository.GetByIdAsync(timeshare.TimeshareStatus.timeshareStatusId);
                        if (timeshare.TimeshareStatus == null)
                        {
                            return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Status not found." });
                        }
                    }

                    // Save changes to the database
                    await _timeshareRepository.Update(timeshare);

                    return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Timeshare confirmed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetTimeshareByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                // Assuming _timeshareRepository has a method to get timeshares by user ID
                var timeshares = await _timeshareRepository.GetByUserId(userId);

                if (timeshares == null || !timeshares.Any())
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "No timeshares found for the user." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = timeshares, IsSucceed = true, Message = "Timeshares retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetTimesharesByStatus/{statusId}")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> GetTimesharesByStatus(int statusId)
        {
            try
            {
                // Retrieve timeshares with the specified statusId
                var timeshares = await _timeshareRepository.GetByStatusId(statusId);

                if (timeshares == null || !timeshares.Any())
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "No timeshares found with the specified statusId." });
                }

                return Ok(new ResponseDTO { Result = timeshares, IsSucceed = true, Message = "Timeshares retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("DeclineTimeshare/{timeshareId}")]
        public async Task<IActionResult> DeclineTimeshare(int timeshareId)
        {
            try
            {
                Timeshare timeshare = await _timeshareRepository.GetById(timeshareId);
                if (timeshare == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found" });
                }
                // Update timeshare status ID directly
                timeshare.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 3 };
                if (timeshare.TimeshareStatus != null && timeshare.TimeshareStatus.timeshareStatusId != null)
                {
                    timeshare.TimeshareStatus = await _timeshareStatusRepository.GetByIdAsync(timeshare.TimeshareStatus.timeshareStatusId);
                    if (timeshare.TimeshareStatus == null)
                    {
                        return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Status not found." });
                    }
                }

                // Save changes to the database
                await _timeshareRepository.Update(timeshare);

                return Ok(new ResponseDTO { Result = timeshare, IsSucceed = true, Message = "Timeshare declined successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetAllActiveTimeshares")]
        public async Task<IActionResult> GetActiveTimeshares()
        {
            try
            {
                // Retrieve timeshares from the repository
                var allTimeshares = await _timeshareRepository.GetAllAsync();

                var activeTimeshares = allTimeshares
                .Where(t => t.PublicDate <= DateTime.Now && t.PublicDate.AddDays(30) >= DateTime.Now &&
                        t.timeshareStatusId == 2)
                .ToList();

                return Ok(new ResponseDTO { Result = activeTimeshares, IsSucceed = true, Message = "Active Timeshares retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetBookingCount")]
        public async Task<ActionResult<IEnumerable<Timeshare>>> GetTimeshares()
        {
            // Retrieve all timeshares
            var timeshares = await _db.Timeshares.ToListAsync();

            // Calculate the count of booking requests for each timeshare
            var timeshareWithBookingCount = timeshares.Select(timeshare =>
            {
                var bookingCount = _db.BookingRequests.Count(b => b.timeshareId == timeshare.timeshareId);
                return new
                {
                    Timeshare = timeshare,
                    BookingCount = bookingCount
                };
            }).ToList();

            return Ok(timeshareWithBookingCount);
        }
    }
}

