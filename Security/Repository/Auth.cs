using Application.Contrast.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Security.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Security.Repository
{
    public class Auth : IAuth
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;


        public Auth(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim("userName" , user?.UserName??""),
            new Claim("firstName", user?.FirstName??""),
            new Claim("lastName",user ?.LastName ?? "")



            };

            var roles = await userManager.GetRolesAsync(user);
            
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));


                }

            


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:SecretKey"]));
            var Credentila = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            double expierAt = Convert.ToDouble(configuration["jwt:DurationInMinutes"]);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expierAt),
                signingCredentials: Credentila
                );

            return new JwtSecurityTokenHandler().WriteToken(token);






        }



        //this version is better compare to firs one

        //private string CreateToken(List<Claim> claims, DateTime dateTime)
        //{
        //    Dictionary<string, object> claimDic = new Dictionary<string, object>();
        //    foreach (var claim in claims)
        //    {
        //        claimDic.Add(claim.Type, claim.Value);

        //    }
        //    var tokeDescriptor = new SecurityTokenDescriptor
        //    {
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["SecretKey"])),
        //        SecurityAlgorithms.HmacSha256Signature),

        //        Claims = claimDic,
        //        Expires = dateTime,
        //        NotBefore = DateTime.UtcNow




        //    };
        //    var tokenHandeler = new JsonWebTokenHandler();
        //    return tokenHandeler.CreateToken(tokeDescriptor);

        //}
    }
}
