using Application.Common.BaseModel;
using Application.DTOs.Security.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IRoleServices
    {
        Task<OperationResult> CreateRoleAsync(CreateRoleDto model);
        Task<GenericOperationResult<List<UpdateRoleDto>>> GetRoles();

        Task<OperationResult> UpdateRolesAsync(CreateRoleDto model,int id);

        Task<OperationResult> DeleteRoleAsync(int id);
    }
}
