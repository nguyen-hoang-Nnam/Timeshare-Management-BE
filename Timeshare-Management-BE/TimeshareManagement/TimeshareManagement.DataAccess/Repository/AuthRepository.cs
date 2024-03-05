using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.DataAccess.Data;
using TimeshareManagement.DataAccess.Repository.IRepository;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;
using TimeshareManagement.Models.Role;

namespace TimeshareManagement.DataAccess.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _db = db;
        }



        public async Task<ResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            /*var user = await _userManager.FindByNameAsync(loginDTO.Username);

            if (user is null)
            {
                return new ResponseDTO() { IsSucceed = true, Message = "Invalid Credential" };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isPasswordCorrect)
            {
                return new ResponseDTO() { IsSucceed = true, Message = "Invalid Credential" };
            }*/

            ApplicationUser user = null;

            // Check if loginDTO is an email address
            if (loginDTO.Username.Contains('@'))
            {
                // If it is, try to find user by email
                user = await _userManager.FindByEmailAsync(loginDTO.Username);

                if (user is null)
                {
                    return new ResponseDTO() { IsSucceed = false, Message = "Invalid Credential" };
                }

                // Allow login without password if email is found
            }
            else
            {
                // If it's not an email, try to find user by username
                user = await _userManager.FindByNameAsync(loginDTO.Username);

                if (user is null)
                {
                    return new ResponseDTO() { IsSucceed = false, Message = "Invalid Credential" };
                }

                // Check password only if the login is done with a username
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                if (!isPasswordCorrect)
                {
                    return new ResponseDTO() { IsSucceed = false, Message = "Invalid Credential" };
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("Name", user.Name),
                /*new Claim("LastName", user.LastName),*/
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);
            return new ResponseDTO() { IsSucceed = true, Message = token };
        }

        public async Task<ResponseDTO> MakeAdminAsync(UpdatePermissionDTO updatePermissionDTO)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDTO.Username);

            if (user is null)
            {
                return new ResponseDTO() { IsSucceed = true, Message = "Invalid Username" };
            }

            var oldRole = await _userManager.GetRolesAsync(user);
            if(oldRole.Any())
            {
                var removeRole = await _userManager.RemoveFromRolesAsync(user, oldRole);
                if (!removeRole.Succeeded)
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Error" };
                }
            }

            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            return new ResponseDTO() { IsSucceed = true, Message = "User is Admin" };
        }

        public async Task<ResponseDTO> MakeOwnerAsync(UpdatePermissionDTO updatePermissionDTO)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDTO.Username);

            if (user is null)
            {
                return new ResponseDTO() { IsSucceed = true, Message = "Invalid Username" };
            }
            var oldRole = await _userManager.GetRolesAsync(user);
            if (oldRole.Any())
            {
                var removeRole = await _userManager.RemoveFromRolesAsync(user, oldRole);
                if (!removeRole.Succeeded)
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Error" };
                }
            }

            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);

            return new ResponseDTO() { IsSucceed = true, Message = "User is Owner" };
        }

        public async Task<ResponseDTO> MakeStaffAsync(UpdatePermissionDTO updatePermissionDTO)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDTO.Username);

            if (user is null)
            {
                return new ResponseDTO() { IsSucceed = true, Message = "Invalid Username" };
            }
            var oldRole = await _userManager.GetRolesAsync(user);
            if (oldRole.Any())
            {
                var removeRole = await _userManager.RemoveFromRolesAsync(user, oldRole);
                if (!removeRole.Succeeded)
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Error" };
                }
            }

            await _userManager.AddToRoleAsync(user, StaticUserRoles.STAFF);

            return new ResponseDTO() { IsSucceed = true, Message = "User is Staff" };
        }

        public async Task<ResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var isExistUser = await _userManager.FindByNameAsync(registerDTO.Username);

            if (isExistUser != null)
            {
                return new ResponseDTO() { IsSucceed = true, Message = "UserName already exist" };
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                Name = registerDTO.Name,
                /*LastName = registerDTO.LastName,*/
                Email = registerDTO.Email,
                UserName = registerDTO.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = registerDTO.PhoneNumber,
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Falied Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new ResponseDTO() { IsSucceed = true, Message = errorString };
            }
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        public async Task<ResponseDTO> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
            bool isStaffRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.STAFF);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists && isStaffRoleExists)
            {
                return new ResponseDTO()
                {
                    IsSucceed = true,
                    Message = "Roles Seeding is already done"
                };
            }
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.STAFF));

            return new ResponseDTO()
            {
                IsSucceed = true,
                Message = "Roles Seeding done successfully"
            };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}
