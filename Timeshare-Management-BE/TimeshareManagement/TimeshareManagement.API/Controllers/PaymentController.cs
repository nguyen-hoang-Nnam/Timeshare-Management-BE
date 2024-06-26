﻿using AutoMapper;
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
        private readonly IUserRepository _userRepository;

        public PaymentController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IBookingRequestRepository bookingRequestRepository, IPaymentRepository paymentRepository, ITimeshareRepository timeshareRepository, IUserRepository userRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _bookingRequestRepository = bookingRequestRepository;
            _paymentRepository = paymentRepository;
            _timeshareRepository = timeshareRepository;
            _userRepository = userRepository;
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
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Payment object is null." });
            }

            if (payment.BookingRequestId == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "BookingRequestId is required." });
            }

            // Retrieve the BookingRequest from the repository
            var bookingRequest = await _bookingRequestRepository.GetByIdAsync(payment.BookingRequestId.Value);

            // Check if the BookingRequest is null
            if (bookingRequest == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Invalid Booking Request" });
            }

            // Ensure that the BookingRequest has an associated Timeshare
            if (bookingRequest.timeshareId == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }

            // Retrieve the Timeshare associated with the BookingRequest
            var timeshare = await _timeshareRepository.GetByIdAsync(bookingRequest.timeshareId.Value);

            // Check if the Timeshare is null
            if (timeshare == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }

            if (bookingRequest.Id == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }

            // Retrieve the Timeshare associated with the BookingRequest
            var user = await _userRepository.GetByIdAsync(bookingRequest.Id);

            // Check if the Timeshare is null
            if (user == null)
            {
                return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare not found for the Booking" });
            }
            bookingRequest.timeshareStatusId = 6;
            await _bookingRequestRepository.Update(bookingRequest);

            payment.Amount = timeshare.CalculatePrice(timeshare.dateFrom, timeshare.dateTo);
            payment.PaymentDate = DateTime.Now;
            payment.timeshareStatusId = 6;


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
        [HttpGet]
        [Route("GetPaymentByBooingId/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBookingId(int bookingId)
        {
            try
            {
                // Assuming _timeshareRepository has a method to get timeshares by user ID
                var payments = await _paymentRepository.GetPaymentByBookingId(bookingId);

                if (payments == null || !payments.Any())
                {
                    return StatusCode(200, new ResponseDTO { Result = null, IsSucceed = false, Message = "No payment found for the booking." });
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
