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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _userRepository;

        public UserController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IRepository<ApplicationUser> userRepository)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var user = await _userRepository.GetAll();
                return Ok(new ResponseDTO { Result = user, IsSucceed = true, Message = "User retrived successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "User not found." });
                }
                else
                {
                    return Ok(new ResponseDTO { Result = user, IsSucceed = true, Message = "User retrieved successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        
        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDTO user)
        {
            try
            {
                var existinguser = await _userRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "User not found." });
                } else
                {
                    // Update user properties
                    existinguser.Email = user.Email;
                    existinguser.PhoneNumber = user.PhoneNumber;
                    existinguser.Name = user.Name;
                    existinguser.NormalizedEmail = user.Email.ToUpper();


                    // Save changes using the generic repository
                    await _userRepository.Update(existinguser);
                }

                return Ok(new ResponseDTO { Result = user, IsSucceed = true, Message = "Update User successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userRepository.DeleteById(id);
                return Ok(new ResponseDTO { Result = null, IsSucceed = true, Message = "Delete User successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUser(int page, int pageSize, string? searchName)
        {
            try
            {
                Expression<Func<ApplicationUser, bool>> filter = null;

                if (!string.IsNullOrEmpty(searchName))
                {
                    filter = entity => entity.UserName.Contains(searchName);
                }
                /*if (searchPrice.HasValue)
                {
                    filter = entity => entity.Price == searchPrice.Value;
                }*/
                var item = await _userRepository.GetPagedAsync(page, pageSize, filter);
                return Ok(new ResponseDTO { Result = item, IsSucceed = true, Message = "Paging User successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
