using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Identity;
using System.Security.Claims;

namespace Shopping.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;



        public UserManagementController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;

        }

        [HttpGet("me")]
        public async Task<IActionResult> me()
        {
            try
            {
                var userIdS = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdS, out int userId))
                {
                    return Unauthorized(
                        OperationResult.ToFail(
                            "Failed",
                            new List<string>
                            {
                "Invalid user identifier"
                            }));
                }
                var user = await userService.GetProfile(userId);
                if (user == null)
                {
                    return NotFound(OperationResult.ToFail("failed ", new List<string> { "there is not such user" }));




                }


                return Ok(user);








            }
            catch
            {
                return StatusCode(500);

            }


        }


        [HttpPut("UpdateMyProfile")]

        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMeModel user)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //chech user
            try
            {
                var result = await userService.UpdateProfile(userid.ToString(), user);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);


            }
            catch (Exception ex) {
                return StatusCode(500);


            }



        }





        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var response = await userService.GetAllUsersAsync();
            if (!response.Success)
            {
                return BadRequest(response);

            }
            return Ok(response);



        }
        //TODO DELETE USERS 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {

            var response = await userService.DeleteUser(id);
            if (!response.Success)
            {
                return BadRequest(response);
                
            }
            return Ok(response);



        }

        



    }
}
