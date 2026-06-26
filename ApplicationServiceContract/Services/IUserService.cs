using Application.Common.BaseModel;
using Application.DTOs.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfile(int userId);
        Task<OperationResult> UpdateProfile( string id ,UpdateMeModel dto);
        Task<GenericOperationResult<List<UserProfileDto>>> GetAllUsersAsync();
        Task<OperationResult> DeleteUser(int id);
    }
}
