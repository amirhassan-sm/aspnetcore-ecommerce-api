using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Security
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string  lastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
