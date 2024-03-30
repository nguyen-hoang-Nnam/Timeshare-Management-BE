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
        private readonly ITimeshareRepository _timeshareRepository;

        public PaymentController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IBookingRequestRepository bookingRequestRepository, IPaymentRepository paymentRepository, ITimeshareRepository timeshareRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _bookingRequestRepository = bookingRequestRepository;
            _paymentRepository = paymentRepository;
            _timeshareRepository = timeshareRepository;
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
        /*[HttpPost]
        [Route("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Payment object is null." });
            }
            *//*payment.TimeshareStatus = new TimeshareStatus { timeshareStatusId = 1 };*//*
            

            if (payment.BookingRequest != null && payment.BookingRequest.bookingRequestId != null)
            {
                payment.BookingRequest = await _bookingRequestRepository.GetByIdAsync(payment.BookingRequest.bookingRequestId);
            }
            else
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Invalid Booking Request" });
            }

            if (payment.BookingRequest.Timeshare != null)
            {
                payment.Amount = payment.BookingRequest.Timeshare.Price;
            }
            else
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }
            await _paymentRepository.Create(payment);

            return Ok(new ResponseDTO { Result = payment, IsSucceed = true, Message = "Payment created successfully" });
        }*/
        [HttpPost]
        [Route("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return StatusCode(400, new ResponseDTO { Result = null, IsSucceed = false, Message = "Payment object is null." });
            }

            if (payment.BookingRequestId == null)
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "BookingRequestId is required." });
            }

            // Retrieve the BookingRequest from the repository
            var bookingRequest = await _bookingRequestRepository.GetByIdAsync(payment.BookingRequestId.Value);

            // Check if the BookingRequest is null
            if (bookingRequest == null)
            {
                return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Invalid Booking Request" });
            }

            // Ensure that the BookingRequest has an associated Timeshare
            if (bookingRequest.timeshareId == null)
            {
                return StatusCode(404, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }

            // Retrieve the Timeshare associated with the BookingRequest
            var timeshare = await _timeshareRepository.GetByIdAsync(bookingRequest.timeshareId.Value);

            // Check if the Timeshare is null
            if (timeshare == null)
            {
                return StatusCode(404, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }

            // Set the payment amount to the Timeshare's price
            payment.Amount = timeshare.CalculatePrice(timeshare.dateFrom, timeshare.dateTo);

            // Set the PaymentDate to the current date and time
            payment.PaymentDate = DateTime.Now;
            // Create the payment
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
        [HttpDelete]
        [Route("DeletePayment/{id:int}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                await _paymentRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete Payment successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
