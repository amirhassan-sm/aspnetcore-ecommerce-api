using Application.Common.BaseModel;
using Application.DTOs.Security.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IUserRoleServices
    {
        Task<OperationResult> AssignRoleAsync(UserRoleDto model);
        Task<OperationResult> RemoveRoleFromUserAsync(UserRoleDto model);
    }
}
