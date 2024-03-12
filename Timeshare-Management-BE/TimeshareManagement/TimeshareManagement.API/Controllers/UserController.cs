using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;
using TimeshareManagement.Models.Role;

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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IConfiguration configuration, ApplicationDbContext db, IMapper mapper, IRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
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
                var existingUser = await _userRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "User not found." });
                } else
                {
                    // Update user properties
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Name = user.Name;
                    existingUser.NormalizedEmail = user.Email.ToUpper();
                    existingUser.isActive = user.isActive;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        var passwordHasher = new PasswordHasher<ApplicationUser>();
                        existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, user.Password);
                    }


                    // Save changes using the generic repository
                    await _userRepository.Update(existingUser);
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
                // Retrieve the user by ID
                var userToDelete = await _userRepository.GetById(id);

                if (userToDelete == null)
                {
                    return NotFound(new ResponseDTO { Result = null, IsSucceed = false, Message = "User not found" });
                }

                // Soft delete by setting isActive to false
                userToDelete.isActive = false;

                // Update the user in the repository
                await _userRepository.Update(userToDelete);
                /*BackgroundJob.Schedule(() => PermanentlyDeleteUser(id), TimeSpan.FromDays(30));*/

                return Ok(new ResponseDTO { Result = userToDelete, IsSucceed = true, Message = "User deactivated successfully" });
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
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO createUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            ApplicationUser user = new ApplicationUser
            {
                UserName = createUser.UserName,
                Email = createUser.Email,
                Name = createUser.Name,
                PhoneNumber = createUser.PhoneNumber,
                isActive = true,
            };
            var result = await _userManager.CreateAsync(user, createUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, StaticUserRoles.STAFF);

                return Ok(new ResponseDTO { Result = user, IsSucceed = true, Message = "Create User successfully" });
            }
            return BadRequest(new ResponseDTO { Result = null, IsSucceed = false, Message = $"Error: {result.Errors}" });
        }
    }
}
