using Application.Contrast.Services;
using Application.DTOs.Security.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Security;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.Admin)]
    public class UserRolesController : ControllerBase
    {

        private readonly IUserRoleServices userRoleServices;
        public UserRolesController(IUserRoleServices userRoleServices)
        {
            this.userRoleServices = userRoleServices;   
            
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole([FromBody]UserRoleDto model) {
            var result = await userRoleServices.AssignRoleAsync(model);
            if (!result.Success)
            {
                return BadRequest(result);
                
            }
            return Ok(result);  

        }
        [HttpDelete("{userid}/{roleid}")]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            var model = new UserRoleDto { RoleId = roleId, UserId = userId };
            var result = await userRoleServices.RemoveRoleFromUserAsync(model);
            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);


        }
    }
}
