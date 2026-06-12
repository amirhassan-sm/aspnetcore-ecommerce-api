//using Application.DTOs.Security;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
//using System.Security.Claims;


//namespace Shopping.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]

//    public class AuthenticationController : ControllerBase

//    {
//        [HttpPost("Authenticate")]
//        public IActionResult Authenticate([FromBody] Application.DTOs.Security.Credential cred)
//        {
//            if (cred.Username == "amir" && cred.Password == "12345")
//            {
//                var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, "amir"),
//                    new Claim(ClaimTypes.Email, "h.soltan.mohseni@gmail.com"),
//                    new Claim(ClaimTypes.Role, "Admin"),


//                };
//                var expireTome = DateTime.UtcNow.AddMinutes(1);

//                return Ok(
//                    new
//                    {
//                        AccssesToken =CreateToken

//                }

//                    );

//            }
//            ;

//ModelState        }

//        private object CreateToken(List<Claim> claims , DateTime dateTime ) {
//            var tokenHandeler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
//            tokenHandeler.CreateToken();
        
//        }



//        [HttpGet("GG")]
//        public IActionResult gg()
//        {

//            return Ok();
//        }
//    }
//}



