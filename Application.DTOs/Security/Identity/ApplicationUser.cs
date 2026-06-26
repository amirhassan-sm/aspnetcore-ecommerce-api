
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Identity
{
    public class ApplicationUser :IdentityUser<int>// this is for primmary key
    {
        public string  FirstName { get; set; }= string.Empty;
        public string  LastName { get; set; } =string.Empty;

        public string? RefreshToken { get; set; } 
        public DateTime RefreshTokenExpiryTime { get; set; }


        public ICollection<AppliactionUserRole> appliactionUserRoles { get; set; }




    }
}
