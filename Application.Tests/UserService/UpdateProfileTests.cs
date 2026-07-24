using Application.Contrast.Repositories;
using Application.DTOs.Security;
using Application.implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Serialization;
using Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.UserService
{
    public class UpdateProfileTests
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<IUserManegment> mockRepo;
        private readonly UserServices service;

        public UpdateProfileTests()
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


        public async Task UpdateProfile_ShouldFail_WhenUserNotExist()
        {
            var id = "1";

            var model = new UpdateMeModel {
                Firstname = "Test", Lastname = "Test",


            };
            var user = new ApplicationUser
            {
                FirstName = "Test",
                LastName = "Test",

            };

            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync((ApplicationUser)null);

            var result = await service.UpdateProfile(id,model);


            Assert.False(result.Success);
            Assert.Equal("User_Not_exist",result.ErrorCode);

            mockUserManager.Verify(x => x.FindByIdAsync(id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Never);







        }

        [Fact]
        public async Task UpdateProfile_ShouldFail_WhenFaieldToChangePassword()
        {
            var id = "1";

            var model = new UpdateMeModel
            {
                Firstname = "Test",
                Lastname = "Test",
                NewPassword = "New",
                CurrentPassword = "Current"


            };
            var user = new ApplicationUser
            {
                FirstName = "Test",
                LastName = "Test",

            };

            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync(user);
            mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "failed to change password" }));

            var result = await service.UpdateProfile(id, model);


            Assert.False(result.Success);
            Assert.Equal("Failed_To_changePassword", result.ErrorCode);

            mockUserManager.Verify(x => x.FindByIdAsync(id), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Never);







        }
        [Fact]
        public async Task UpdateProfile_ShouldFail_WhenUpdateFailed()
        {
            var id = "1";

            var model = new UpdateMeModel
            {
                Firstname = "Test",
                Lastname = "Test",
                NewPassword = "New",
                CurrentPassword = "Current"


            };
            var user = new ApplicationUser
            {
                FirstName = "Test",
                LastName = "Test",

            };

            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync(user);
            mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed());

            var result = await service.UpdateProfile(id, model);


            Assert.False(result.Success);
            Assert.Equal("UpdateUser_Failed",result.ErrorCode);

            mockUserManager.Verify(x => x.FindByIdAsync(id), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);







        }

        [Fact]
        public async Task UpdateProfile_ShouldSuceed()
        {
            var id = "1";

            var model = new UpdateMeModel
            {
                Firstname = "Test",
                Lastname = "Test",
                NewPassword = "New",
                CurrentPassword = "Current"


            };
            var user = new ApplicationUser
            {
                FirstName = "Test",
                LastName = "Test",

            };

            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync(user);
            mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await service.UpdateProfile(id, model);


            Assert.True(result.Success);

            mockUserManager.Verify(x => x.FindByIdAsync(id), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);







        }

        [Fact]
        public async Task UpdateProfile_ShouldFail_WhenExceptionThrows()
        {
            var id = "1";

            var model = new UpdateMeModel
            {
                Firstname = "Test",
                Lastname = "Test",
                NewPassword = "New",
                CurrentPassword = "Current"


            };
            var user = new ApplicationUser
            {
                FirstName = "Test",
                LastName = "Test",

            };

            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync(user);
            mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(x => x.UpdateAsync(user)).ThrowsAsync(new Exception());

            var result = await service.UpdateProfile(id, model);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured", result.ErrorCode);

            mockUserManager.Verify(x => x.FindByIdAsync(id), Times.Once);
            mockUserManager.Verify(x => x.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);







        }





    }
}
