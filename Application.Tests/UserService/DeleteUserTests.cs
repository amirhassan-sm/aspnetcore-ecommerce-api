using Application.Contrast.Repositories;
using Application.implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly UserServices service;

        public DeleteUserTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new List<IUserValidator<ApplicationUser>>(),
                new List<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            mockRepo = new Mock<IUserManegment>();

            service = new UserServices(
                mockUserManager.Object,
                mockRepo.Object);
        }


        [Fact]
        public async Task DeleteUser_ShouldFaile_WhenUserDoesNotExist() 
        {
            var id = 1;


            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync((ApplicationUser)null);


            var result = await service.DeleteUser(id);


            Assert.False(result.Success);
            Assert.Equal("User_Not_Exist",result.ErrorCode);

            mockUserManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
            
        }


        [Fact]
        public async Task DeleteUser_ShouldFaile_WhenIdentityCntNotDeleteUser()
        {
            var id = 1;
            var user = new ApplicationUser { Id = 1, FirstName = "test" };


            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);

            mockUserManager.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed( new IdentityError { Description="Delete Failed"}) );


            var result = await service.DeleteUser(id);


            Assert.False(result.Success);
            Assert.Equal("Delete_Failed", result.ErrorCode);

            mockUserManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);

        }


        [Fact]
        public async Task DeleteUser_ShouldSuceed()
        {
            var id = 1;
            var user = new ApplicationUser { Id = 1, FirstName = "test" };


            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);

            mockUserManager.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success );


            var result = await service.DeleteUser(id);


            Assert.True(result.Success);
            

            mockUserManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);

        }

        [Fact]
        public async Task DeleteUser_ShouldFail_WhenExceptionThrows()
        {
            var id = 1;
            var user = new ApplicationUser { Id = 1, FirstName = "test" };


            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);

            mockUserManager.Setup(x => x.DeleteAsync(user)).ThrowsAsync(new Exception { });


            var result = await service.DeleteUser(id);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);


            mockUserManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);

        }



    }
}
