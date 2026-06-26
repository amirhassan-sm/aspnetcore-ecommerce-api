using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Security
{
    public class AuthTokenResult
    {
        public string?  AccessToken{ get; set; }

        public string? RefreshToken{ get; set; }
    }
}
