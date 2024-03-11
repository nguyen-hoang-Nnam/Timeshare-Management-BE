using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.API.Controllers
{
    [Route("api/BookingRequest")]
    [ApiController]
    public class BookingRequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IBookingRequestRepository _bookingRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITimeshareRepository _timeshareRepository;
        private readonly ITimeshareStatusRepository _timeshareStatusRepository;

        public BookingRequestController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IBookingRequestRepository bookingRequestRepository, IUserRepository userRepository, ITimeshareRepository timeshareRepository, ITimeshareStatusRepository timeshareStatusRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _bookingRequestRepository = bookingRequestRepository;
            _userRepository = userRepository;
            _timeshareRepository = timeshareRepository;
            _timeshareStatusRepository = timeshareStatusRepository;
        }
        [HttpGet]
        [Route("GetAllBookingRequest")]
        public async Task<IActionResult> GetAllBookingRequest()
        {
            try
            {
                var bookingRequest = await _bookingRequestRepository.GetAll();
                return Ok(new ResponseDTO { Result = bookingRequest, IsSucceed = true, Message = "Room retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetRequestById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var request = await _bookingRequestRepository.GetById(id);
                if (request == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Place not found." });
                }
                else
                { 
                    return Ok(new ResponseDTO { Result = request, IsSucceed = true, Message = "Place retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("CreateBookingRequest")]
        public async Task<IActionResult> CreateBookingRequest([FromBody] BookingRequest bookingRequest)
        {
            if (bookingRequest == null)
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare object is null." });
            }
            bookingRequest.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 1 };
            if (bookingRequest.User != null && bookingRequest.User.Id != null)
            {
                bookingRequest.User = await _userRepository.GetByIdAsync(bookingRequest.User.Id);
                if (bookingRequest.User == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "User not found." });
                }
            }

            if (bookingRequest.Timeshare != null && bookingRequest.Timeshare.timeshareId != null)
            {
                bookingRequest.Timeshare = await _timeshareRepository.GetByIdAsync(bookingRequest.Timeshare.timeshareId);
                if (bookingRequest.Timeshare == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found." });
                }
            }
            if (bookingRequest.TimeshareStatus != null && bookingRequest.TimeshareStatus.timeshareStatusId != null)
            {
                bookingRequest.TimeshareStatus = await _timeshareStatusRepository.GetByIdAsync(bookingRequest.TimeshareStatus.timeshareStatusId);
                if (bookingRequest.TimeshareStatus == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Status not found." });
                }
            }
            await _bookingRequestRepository.Create(bookingRequest);

            return Ok(new ResponseDTO { Result = bookingRequest, IsSucceed = true, Message = "Booking created successfully" });
        }
        [HttpDelete]
        [Route("DeletePlace/{id:int}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                await _bookingRequestRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete Booking successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] BookingRequest bookingRequest)
        {
            await _bookingRequestRepository.SendBookingConfirmationAsync(bookingRequest);
            return Ok("Send Email successfully");
        }
        [HttpGet]
        [Route("GetBookingByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                // Assuming _timeshareRepository has a method to get timeshares by user ID
                var booking = await _bookingRequestRepository.GetByUserId(userId);

                if (booking == null || !booking.Any())
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "No timeshares found for the user." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = booking, IsSucceed = true, Message = "Timeshares retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetBookingByStatus/{statusId}")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> GetBookingByStatus(int statusId)
        {
            try
            {
                // Retrieve timeshares with the specified statusId
                var booking = await _bookingRequestRepository.GetByStatusId(statusId);

                if (booking == null || !booking.Any())
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "No booking request found with the specified statusId." });
                }

                return Ok(new ResponseDTO { Result = booking, IsSucceed = true, Message = "Booking request retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("ConfirmBooking/{bookingId}")]
        public async Task<IActionResult> ConfirmBooking(int bookingId)
        {
            try
            {
                BookingRequest booking = await _bookingRequestRepository.GetById(bookingId);
                if (booking == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Booking not found" });
                }
                // Update timeshare status ID directly
                booking.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 2 };
                if (booking.TimeshareStatus != null && booking.TimeshareStatus.timeshareStatusId != null)
                {
                    booking.TimeshareStatus = await _timeshareStatusRepository.GetByIdAsync(booking.TimeshareStatus.timeshareStatusId);
                    if (booking.TimeshareStatus == null)
                    {
                        return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Status not found." });
                    }
                }

                // Save changes to the database
                await _bookingRequestRepository.Update(booking);

                return Ok(new ResponseDTO { Result = booking, IsSucceed = true, Message = "Booking confirmed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("DeclineTimeshare/{bookingId}")]
        public async Task<IActionResult> DeclineBooking(int bookingId)
        {
            try
            {
                BookingRequest booking = await _bookingRequestRepository.GetById(bookingId);
                if (booking == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Booking not found" });
                }
                // Update timeshare status ID directly
                booking.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 3 };
                if (booking.TimeshareStatus != null && booking.TimeshareStatus.timeshareStatusId != null)
                {
                    booking.TimeshareStatus = await _timeshareStatusRepository.GetByIdAsync(booking.TimeshareStatus.timeshareStatusId);
                    if (booking.TimeshareStatus == null)
                    {
                        return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Status not found." });
                    }
                }

                // Save changes to the database
                await _bookingRequestRepository.Update(booking);

                return Ok(new ResponseDTO { Result = booking, IsSucceed = true, Message = "Booking declined successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetBookingByTimeshare/{timeshareId}")]
        /*[Authorize(Roles = StaticUserRoles.ADMIN)]*/
        public async Task<IActionResult> GetBookingByTimeshare(int timeshareId)
        {
            try
            {
                // Retrieve timeshares with the specified statusId
                var booking = await _bookingRequestRepository.GetByTimeshareIdAndStatusId(timeshareId, 1);

                if (booking == null || !booking.Any())
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "No booking request found with the specified timeshareId." });
                }

                return Ok(new ResponseDTO { Result = booking, IsSucceed = true, Message = "Booking request retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
