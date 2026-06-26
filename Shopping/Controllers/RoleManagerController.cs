using Application.Contrast.Services;
using Application.DTOs.Security.Roles;
using Application.implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RoleManagerController : ControllerBase
    {
        private readonly IRoleServices roleServices;
        private readonly ILogger<RoleServices> logger; 
        public RoleManagerController(IRoleServices roleServices, ILogger<RoleServices> logger)
        {
            this.roleServices = roleServices;
            this.logger = logger;

        }
        
        [HttpGet("roles")]

        public async Task<IActionResult> roles()
        {
            try
            {
                var result = await roleServices.GetRoles();
                if (!result.Success)
                {
                    return BadRequest(result);

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }
        [HttpPost("roles")]
        public async Task<IActionResult> roles([FromBody] CreateRoleDto model)
        {
            try
            {
                var result = await roleServices.CreateRoleAsync(model);
                if (!result.Success)
                {
                    return BadRequest(result);

                }

                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500);
            }

        }

        [HttpPut("roles/{id}")]
        public async Task<IActionResult> roles([FromBody] CreateRoleDto model, int id)
        {
            try
            {
                var result = await roleServices.UpdateRolesAsync(model, id);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex) {
                logger.LogError(ex.Message,"update failed" );
                return StatusCode(500);

                
               
            }


        }

        [HttpDelete("roles/{id}")]

        public async Task<IActionResult> roles(int id)
        {
            try
            {
                var result = await roleServices.DeleteRoleAsync(id);
                if (!result.Success)
                {
                    return BadRequest(result);
                    
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, "delete failed");
               return StatusCode(500);
            }

        }
    }
}
