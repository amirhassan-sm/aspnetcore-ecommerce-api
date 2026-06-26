using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Security.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.implementation
{
    public class UserRoleServices : IUserRoleServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<UserRoleServices> logger;
        public UserRoleServices(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<UserRoleServices> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;

        }
        public async Task<OperationResult> AssignRoleAsync(UserRoleDto model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail("failed to assign", new List<string> { "this user does not exist" });


                }
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {

                    return OperationResult.ToFail("failed to assign", new List<string> { "this role does not exist" });

                }
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    return OperationResult.ToFail("failed to assign role", new List<string> { "user  already has this role" });

                }
                var result = await userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("add failed ", result.Errors.Select(x => x.Description).ToList());


                }
                return OperationResult.ToSuccess(user.Id, $"the role: {role.Name} successfully assigned to user: {user.UserName}");
            }
            catch (Exception ex)
            {
                logger.LogError(
    ex,
    "Assign role failed. UserId: {UserId}, RoleId: {RoleId}",
    model.UserId,
    model.RoleId);
                return OperationResult.ToFail("failed to assign role", new List<string> { "an uexpected error occured " });

            }


        }

        public async Task<OperationResult> RemoveRoleFromUserAsync(UserRoleDto model)
        {
            try
            {

                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail("failed to remove", new List<string> { "this user does not exist" });


                }
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {

                    return OperationResult.ToFail("failed to remove", new List<string> { "this role does not exist" });

                }
                if (!await userManager.IsInRoleAsync(user, role.Name))
                {
                    return OperationResult.ToFail("failed to remove role from this user",
                        new List<string> { $"the user{user.UserName} does not assigned to the role {role.Name}" });

                }
                var result = await userManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to remove this role", result.Errors.Select(x => x.Description).ToList());

                }
                return OperationResult.ToSuccess(user.Id, $"the role : {role.Name} removed successfully from the user:{user.UserName}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to remove role from user", model.UserId, model.RoleId);
                return OperationResult.ToFail("Failed to remove role", new List<string> { "an uexpected error occured " });

            }



        }
    }
}
