using Application.Common.BaseModel;
using Application.Contrast.Repositories;
using Application.DTOs.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Security.Identity;
using Security.Repository;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AuthenticationController : ControllerBase

    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuth auth;
        public AuthenticationController(IConfiguration configuration, UserManager<ApplicationUser> userManager, IAuth auth)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.auth = auth;

        }


        [HttpPost("signUp")]

        public async Task<IActionResult> signUp(RegisterDto model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.lastName
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(OperationResult.
                        ToFail("sign Up failed" , result.Errors.Select(x=>x.Description ).ToList()));
                }
                var jwt = await auth.GenerateToken(user);
                var refresh =  auth.GenerateRefreshToken();
                user.RefreshToken=refresh;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                //user.RefreshTokenExpiryTime.AddDays(7);
                await userManager.UpdateAsync(user);
                


                return Ok(  GenericOperationResult<AuthTokenResult>.ToSuccess("signed up successfully " , new AuthTokenResult
                {AccessToken= jwt , 
                RefreshToken = refresh
                    
                } ));
            }
            catch (Exception ex)
            {

                return StatusCode(500);

            }

        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Application.DTOs.Security.LoginDto model)
        {
            try
            {

                var user = await userManager.FindByNameAsync(model.UserName);

                if (user == null) { return Unauthorized(OperationResult.ToFail("failed", new List<string> { "this user dose not exist" })); }

                var ValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

                if (!ValidPassword) { return Unauthorized(OperationResult.ToFail("failed", new List<string> { "invalid username or password" })); }



                var token = await auth.GenerateToken(user);

                var refreshToken = auth.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);


                await userManager.UpdateAsync(user);




                return Ok(GenericOperationResult<AuthTokenResult>.ToSuccess("signed up successfully ", new AuthTokenResult
                {
                    AccessToken = token,
                    RefreshToken = refreshToken

                }));
            }
            catch (Exception ex) { return BadRequest(ex); }
            ;
        }

        [HttpPost("Refresh")]

         public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshToken) 
        { 
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow) {
            return Unauthorized("the refresh token is invalid or expierd");
            
            }
            var token = await auth.GenerateToken(user);
            
            var newRefreshToken =  auth.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await userManager.UpdateAsync(user);
            return Ok(GenericOperationResult<AuthTokenResult>.ToSuccess("signed up successfully ", new AuthTokenResult
            {
                AccessToken = token,
                RefreshToken = newRefreshToken

            }));



        }







    }
}



