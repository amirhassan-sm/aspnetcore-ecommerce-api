using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Identity;
using Security.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Contrast.Repositories;

namespace Application.implementation
{
    public class UserServices : IUserService
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserManegment userManegmentRepo;

        
        public UserServices(UserManager<ApplicationUser> userManager, IUserManegment userManegmentRepo)
        {
            this.userManager = userManager;
            this.userManegmentRepo= userManegmentRepo;

        }

        public async Task<OperationResult> DeleteUser(int id)
        {
            try
            {
                
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail("failed to delete", new List<string> { "this user does not exist" });

                }
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail(user.Id, "failed to delete", result.Errors.Select(x => x.Description).ToList());


                }
                return OperationResult.ToSuccess(id, "user deleted");

            }
            catch (Exception ex) {
                return OperationResult.ToFail("failed to delete", new List<string> { "an unexpected error occured"});
            
            }
        }

        public  async Task<GenericOperationResult<List<UserProfileDto>>> GetAllUsersAsync()
        {
            try
            {
                var result = await userManegmentRepo.GetAllUsersQuery();

                return GenericOperationResult<List<UserProfileDto>>.ToSuccess("Users List", result);

            }
            catch (Exception ex) {
                return GenericOperationResult<List<UserProfileDto>>
                    .ToFail("failed to get list", new List<string> {" an unexpected error occured"});




            }
        }

        public async Task<UserProfileDto> GetProfile(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var roles = await userManager.GetRolesAsync(user);
            List<string> RoleList = new List<string>();
            foreach (string role in roles)
            {
                RoleList.Add(role);


            }
            return new UserProfileDto
            {
                UserId = userId,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                UserName = user.UserName,
                Roles = RoleList

            };


        }

        public async Task<OperationResult> UpdateProfile(string id, UpdateMeModel dto)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return OperationResult.ToFail("update failed", new List<string> { "this User dose not exist" });

                }
                user.UserName = dto.UserName;

                user.FirstName = dto.Firstname;
                user.LastName = dto.Lastname;
                if (!string.IsNullOrEmpty(dto.NewPassword))
                {
                    var resultPass = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                    if (!resultPass.Succeeded)
                    {
                        var error = resultPass.Errors.Select(x => x.Description).ToList();
                        return OperationResult.ToFail("failed to update", error);

                    }

                }



                var result = await userManager.UpdateAsync(user);


                if (result.Succeeded)
                {
                    return OperationResult.ToSuccess(user.Id, "User updated successfully");


                }
                var errors = new List<string>();
                errors = result.Errors.Select(x => x.Description).ToList();


                return OperationResult.ToFail(user.Id, "failed to update User", errors);
            }
            catch (Exception ex)
            {
                return OperationResult.ToFail("update failed", new List<string> { ex.Message });

            }
        }
    }
}
