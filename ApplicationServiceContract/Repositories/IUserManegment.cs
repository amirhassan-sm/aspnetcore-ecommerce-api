using Application.DTOs.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Repositories
{
    public interface IUserManegment
    {
        Task<List<UserProfileDto>> GetAllUsersQuery();

    }
}
