using Application.DTOs.Security.Roles;
using Application.implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Security.Identity;

namespace Application.Tests.UserRoleService
{
    public class AssignRoleTests
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<RoleManager<ApplicationRole>> mockRoleManager;
        private readonly Mock<ILogger<UserRoleServices>> mockLogger;

        private readonly UserRoleServices service;


        public AssignRoleTests()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStore.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);


            var roleStore = new Mock<IRoleStore<ApplicationRole>>();

            mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
                roleStore.Object,
                new List<IRoleValidator<ApplicationRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);


            mockLogger = new Mock<ILogger<UserRoleServices>>();

            service = new UserRoleServices(
                mockUserManager.Object,
                mockRoleManager.Object,
                mockLogger.Object);
        }


        [Fact]
        public async Task AssignRole_ShouldFail_WhenUserDoesNotExist()
        {
            var model = new UserRoleDto
            {
                UserId = 1,
                RoleId = 1
            };


            mockUserManager
                .Setup(x => x.FindByIdAsync(model.UserId.ToString()))
                .ReturnsAsync((ApplicationUser)null);


            var result = await service.AssignRoleAsync(model);


            Assert.False(result.Success);

            mockUserManager.Verify(
                x => x.FindByIdAsync(model.UserId.ToString()),
                Times.Once);

            mockRoleManager.Verify(
                x => x.FindByIdAsync(It.IsAny<string>()),
                Times.Never);
        }


        [Fact]
        public async Task AssignRole_ShouldFail_WhenRoleDoesNotExist()
        {
            var model = new UserRoleDto
            {
                UserId = 1,
                RoleId = 1
            };

            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "test"
            };


            mockUserManager
                .Setup(x => x.FindByIdAsync(model.UserId.ToString()))
                .ReturnsAsync(user);


            mockRoleManager
                .Setup(x => x.FindByIdAsync(model.RoleId.ToString()))
                .ReturnsAsync((ApplicationRole)null);



            var result = await service.AssignRoleAsync(model);



            Assert.False(result.Success);

            mockRoleManager.Verify(
                x => x.FindByIdAsync(model.RoleId.ToString()),
                Times.Once);

            mockUserManager.Verify(
                x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }



        [Fact]
        public async Task AssignRole_ShouldFail_WhenUserAlreadyHasRole()
        {
            var model = new UserRoleDto
            {
                UserId = 1,
                RoleId = 1
            };

            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "test"
            };

            var role = new ApplicationRole
            {
                Id = 1,
                Name = "Admin"
            };


            mockUserManager
                .Setup(x => x.FindByIdAsync(model.UserId.ToString()))
                .ReturnsAsync(user);


            mockRoleManager
                .Setup(x => x.FindByIdAsync(model.RoleId.ToString()))
                .ReturnsAsync(role);


            mockUserManager
                .Setup(x => x.IsInRoleAsync(user, role.Name))
                .ReturnsAsync(true);



            var result = await service.AssignRoleAsync(model);



            Assert.False(result.Success);

            mockUserManager.Verify(
                x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }



        [Fact]
        public async Task AssignRole_ShouldFail_WhenAddRoleFails()
        {
            var model = new UserRoleDto
            {
                UserId = 1,
                RoleId = 1
            };


            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "test"
            };

            var role = new ApplicationRole
            {
                Id = 1,
                Name = "Admin"
            };


            mockUserManager
                .Setup(x => x.FindByIdAsync(model.UserId.ToString()))
                .ReturnsAsync(user);


            mockRoleManager
                .Setup(x => x.FindByIdAsync(model.RoleId.ToString()))
                .ReturnsAsync(role);


            mockUserManager
                .Setup(x => x.IsInRoleAsync(user, role.Name))
                .ReturnsAsync(false);


            mockUserManager
                .Setup(x => x.AddToRoleAsync(user, role.Name))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Add role failed"
                        }));


            var result = await service.AssignRoleAsync(model);



            Assert.False(result.Success);
            Assert.Contains("Add role failed", result.Errors);


            mockUserManager.Verify(
                x => x.AddToRoleAsync(user, role.Name),
                Times.Once);
        }




        [Fact]
        public async Task AssignRole_ShouldSucceed()
        {
            var model = new UserRoleDto
            {
                UserId = 1,
                RoleId = 1
            };


            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "test"
            };

            var role = new ApplicationRole
            {
                Id = 1,
                Name = "Admin"
            };


            mockUserManager
                .Setup(x => x.FindByIdAsync(model.UserId.ToString()))
                .ReturnsAsync(user);


            mockRoleManager
                .Setup(x => x.FindByIdAsync(model.RoleId.ToString()))
                .ReturnsAsync(role);


            mockUserManager
                .Setup(x => x.IsInRoleAsync(user, role.Name))
                .ReturnsAsync(false);


            mockUserManager
                .Setup(x => x.AddToRoleAsync(user, role.Name))
                .ReturnsAsync(IdentityResult.Success);



            var result = await service.AssignRoleAsync(model);



            Assert.True(result.Success);


            mockUserManager.Verify(
                x => x.AddToRoleAsync(user, role.Name),
                Times.Once);
        }




        [Fact]
        public async Task AssignRole_ShouldFail_WhenExceptionOccurs()
        {
            var model = new UserRoleDto
            {
                UserId = 1,
                RoleId = 1
            };


            mockUserManager
                .Setup(x => x.FindByIdAsync(model.UserId.ToString()))
                .ThrowsAsync(new Exception());


            var result = await service.AssignRoleAsync(model);



            Assert.False(result.Success);
        }
    }
}