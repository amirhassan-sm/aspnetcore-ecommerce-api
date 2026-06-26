using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Security.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Security.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.implementation
{
    public class RoleServices : IRoleServices
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        public RoleServices(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<OperationResult> CreateRoleAsync(CreateRoleDto model)
        {
            if (await roleManager.RoleExistsAsync(model.RoleName))
            {
                return OperationResult.ToFail("failed to add the role", new List<string> { "this role already exist" });

            }
            var role = new ApplicationRole();
            role.Name = model.RoleName;
            role.Description = model.Description;
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return OperationResult.ToFail("failed to add role", result.Errors.Select(x => x.Description).ToList());

            }
            return OperationResult.ToSuccess(role.Id, "role added successfully");

        }

        public async Task<OperationResult> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return OperationResult.ToFail("delete failed ", new List<string> { "this role does not exist" });

                }
                var result = await roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("delete failed ", result.Errors.Select(x => x.Description).ToList());


                }
                return OperationResult.ToSuccess(role.Id, "role removed successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.ToFail("delete failed", new List<string> { ex.Message });
            }
        }

        public async Task<GenericOperationResult<List<UpdateRoleDto>>> GetRoles()
        {
            try
            {
                var roles = await roleManager.Roles.Select(x =>
                new UpdateRoleDto
                {
                    RoleId = x.Id,
                    RoleName = x.Name ?? "",
                    Description = x.Description
                }).ToListAsync();

                return GenericOperationResult<List<UpdateRoleDto>>.ToSuccess("roles list", roles);
            }
            catch (Exception ex)
            {
                return GenericOperationResult<List<UpdateRoleDto>>.ToFail("get failed", new List<string> { "an unexpected error occured" });

            }










        }

        public async Task<OperationResult> UpdateRolesAsync(CreateRoleDto model , int id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this role does not exist" });

                }
                role.Name = model.RoleName;
                role.Description = model.Description;
                var result = await roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to update role", result.Errors.Select(x => x.Description).ToList());


                }
                return OperationResult.ToSuccess(role.Id, "role updated successfully");
            }
            catch(Exception ex) 

            {
                

                return OperationResult.ToFail("update failed", new List<string> { "an unecpected error occured" });


            }


        }
    }
}
