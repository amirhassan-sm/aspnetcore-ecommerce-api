using Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Repositories
{
    public interface IAuth
    {
        Task<string> GenerateToken(ApplicationUser user);
       string GenerateRefreshToken();

    }
}
