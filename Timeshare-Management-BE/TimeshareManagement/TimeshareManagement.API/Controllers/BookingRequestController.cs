using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Migrations;
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

        public BookingRequestController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IBookingRequestRepository bookingRequestRepository, IUserRepository userRepository, ITimeshareRepository timeshareRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _bookingRequestRepository = bookingRequestRepository;
            _userRepository = userRepository;
            _timeshareRepository = timeshareRepository;
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

            IEnumerable<ApplicationUser> users = _userRepository.GetAllItem();
            if (bookingRequest.User != null)
            {
                bookingRequest.User = users.FirstOrDefault(u => u.UserName == bookingRequest.User.UserName);
            }
            else
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare object is null." });
            }

            IEnumerable<Timeshare> timeshares = _timeshareRepository.GetAllItem();
            if (bookingRequest.Timeshare != null)
            {
                bookingRequest.Timeshare = timeshares.FirstOrDefault(t => t.timeshareName == bookingRequest.Timeshare.timeshareName);
            }
            else
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare object is null." });
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
    }
}
