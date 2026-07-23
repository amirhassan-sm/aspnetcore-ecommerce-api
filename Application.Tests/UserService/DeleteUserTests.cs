using Application.Contrast.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.UserService
{
    public class DeleteUserTests
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<IUserManegment> mockRepo;
        public DeleteUserTests()
        {
            mockUserManager= new Mock<UserManager<ApplicationUser>>();
            mockRepo= new Mock<IUserManegment>();


        }
    }
}
