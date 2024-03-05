using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.DataAccess.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<ResponseDTO> SeedRolesAsync();
        Task<ResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<ResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<ResponseDTO> MakeAdminAsync(UpdatePermissionDTO updatePermissionDTO);
        Task<ResponseDTO> MakeOwnerAsync(UpdatePermissionDTO updatePermissionDTO);
        Task<ResponseDTO> MakeStaffAsync(UpdatePermissionDTO updatePermissionDTO);
    }
}
