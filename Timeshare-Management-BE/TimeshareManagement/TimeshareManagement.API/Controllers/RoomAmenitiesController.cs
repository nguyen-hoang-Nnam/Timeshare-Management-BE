using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Migrations;
using TimeshareManagement.DataAccess.Repository;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.API.Controllers
{
    [Route("api/roomAmenities")]
    [ApiController]
    public class RoomAmenitiesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IRoomAmenitiesRepository _roomAmenitiesRepository;
       /* private readonly IRoomDetailRepository _roomDetailRepository;*/

        public RoomAmenitiesController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IRoomAmenitiesRepository roomAmenitiesRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _roomAmenitiesRepository = roomAmenitiesRepository;
        }
        [HttpGet]
        [Route("GetAllRoomAmenities")]
        public async Task<IActionResult> GetAllRoomAmenities()
        {
            try  
            {
                var roomAmenities = await _roomAmenitiesRepository.GetAll();
                return Ok(new ResponseDTO { Result = roomAmenities, IsSucceed = true, Message = "Room Amenities retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetRoomAmenitiesById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var roomAmenities = await _roomAmenitiesRepository.GetById(id);
                if (roomAmenities == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Room Amenities not found." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = roomAmenities, IsSucceed = true, Message = "Room Amenities retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("CreateRoomAmenities")]
        public async Task<IActionResult> CreateRoom([FromBody] RoomAmenities roomAmenities)
        {
            try
            {
                if (roomAmenities == null)
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare Detail object is null." });
                }

                /*IEnumerable<RoomDetail> roomDetails = _roomDetailRepository.GetAllItem();
                if (roomAmenities.RoomDetail != null)
                {
                    roomAmenities.RoomDetail = roomDetails.FirstOrDefault(rd => rd.roomDetailId == roomAmenities.RoomDetail.roomDetailId);
                }
                else
                {
                    return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = "Timeshare Detail object is null." });
                }*/
                await _roomAmenitiesRepository.Create(roomAmenities);
                return Ok(new ResponseDTO { Result = roomAmenities, IsSucceed = true, Message = "Create Room Amenities successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpPut]
        [Route("UpdateRoomAmenities/{id:int}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomAmenities roomAmenities)
        {
            try
            {
                var existingRoomAmenities = await _roomAmenitiesRepository.GetById(id);
                if (existingRoomAmenities == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "Room Amenities not found." });
                }
                else
                {
                    existingRoomAmenities.roomAmenitiesName = roomAmenities.roomAmenitiesName;
                    /*existingRoomAmenities.roomDetailId = roomAmenities.roomDetailId;*/

                    await _roomAmenitiesRepository.Update(existingRoomAmenities);
                }
                return Ok(new ResponseDTO { Result = roomAmenities, IsSucceed = true, Message = "Update Room Amenities successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete]
        [Route("DeleteRoomAmenities/{id:int}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                await _roomAmenitiesRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete Room Amenities successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPlace(int page, int pageSize, decimal? searchPrice)
        {
            try
            {
                Expression<Func<RoomAmenities, bool>> filter = null;

                /*if (!string.IsNullOrEmpty(searchName))
                {
                    filter = entity => entity.Name.Contains(searchName);
                }*/
                /*if (searchPrice.HasValue)
                {
                    filter = entity => entity.Price == searchPrice.Value;
                }*/
                var item = await _roomAmenitiesRepository.GetPagedAsync(page, pageSize, filter);
                return Ok(new ResponseDTO { Result = item, IsSucceed = true, Message = "Paging Room Amenities successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
