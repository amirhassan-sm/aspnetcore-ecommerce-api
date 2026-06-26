using Application.Contrast.Repositories;
using Application.DTOs.Security;
using Microsoft.EntityFrameworkCore;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Security.Repository
{

    public class Usermanagement : IUserManegment
    {

        private readonly SecurityContext db;
        public Usermanagement(SecurityContext db)
        {
            this.db = db;

        }
        public async Task<List<UserProfileDto>> GetAllUsersQuery()
        {
            var usersWithRoles =
             from user in db.Users
             join userRole in db.UserRoles
                 on user.Id equals userRole.UserId into userRolesGroup
             from userRole in userRolesGroup.DefaultIfEmpty()

             join role in db.Roles
                 on userRole.RoleId equals role.Id into rolesGroup
             from role in rolesGroup.DefaultIfEmpty()

             select new
             {
                 UserId = user.Id,
                 UserName = user.UserName,
                 Firstname = user.FirstName,
                 Lastname = user.LastName,
                 RoleName = role != null ? role.Name : null
             };
            var result =
                usersWithRoles.GroupBy(x => new { x.UserId })
                .Select(x => new UserProfileDto
                {
                    UserId = x.Key.UserId,
                    Firstname = x.First().Firstname,
                    UserName = x.First().UserName

                ,
                    Lastname = x.First().Lastname,
                    Roles = x.Select(x => x.RoleName).ToList()


                });
            return await result.ToListAsync();





        }
    }
}
