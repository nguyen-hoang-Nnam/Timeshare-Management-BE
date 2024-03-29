using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models.DTO;
using TimeshareManagement.Models.Models;

namespace TimeshareManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IBookingRequestRepository _bookingRequestRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IBookingRequestRepository bookingRequestRepository, IPaymentRepository paymentRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _bookingRequestRepository = bookingRequestRepository;
            _paymentRepository = paymentRepository;
        }
        [HttpGet]
        [Route("GetAllPayment")]
        public async Task<IActionResult> GetAllPayment()
        {
            try
            {
                var payment = await _paymentRepository.GetAll();
                return Ok(new ResponseDTO { Result = payment, IsSucceed = true, Message = "Payment retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetPaymentById/{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var place = await _paymentRepository.GetById(id);
                if (place == null)
                {
                    return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Payment not found." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = place, IsSucceed = true, Message = "Playment retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Payment object is null." });
            }
            /*payment.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 1 };*/
            
            if (payment.BookingRequest != null && payment.BookingRequest.bookingRequestId != null)
            {
                payment.BookingRequest = await _bookingRequestRepository.GetByIdAsync(payment.BookingRequest.bookingRequestId);
                if (payment.BookingRequest == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Booking not found." });
                }
            }
            await _paymentRepository.Create(payment);

            return Ok(new ResponseDTO { Result = payment, IsSucceed = true, Message = "Payment created successfully" });
        }
        [HttpGet]
        [Route("GetPaymentByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                // Assuming _timeshareRepository has a method to get timeshares by user ID
                var payments = await _paymentRepository.GetByUserId(userId);

                if (payments == null || !payments.Any())
                {
                    return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "No payment found for the user." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = payments, IsSucceed = true, Message = "Payment retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
