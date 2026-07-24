using Application.DTOs.Security.Roles;
using Application.implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Security.Identity;

namespace Application.Tests.RoleService
{
    public class CreateRoleTests
    {
        private readonly Mock<RoleManager<ApplicationRole>> mockRoleManager;
        private readonly RoleServices service;

        public CreateRoleTests()
        {
            var store = new Mock<IRoleStore<ApplicationRole>>();

            mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
                store.Object,
                new List<IRoleValidator<ApplicationRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);

            service = new RoleServices(mockRoleManager.Object);
        }

        [Fact]
        public async Task CreateRole_ShouldFail_WhenRoleAlreadyExists()
        {
            // Arrange
            var model = new CreateRoleDto
            {
                RoleName = "Admin",
                Description = "Administrator"
            };

            mockRoleManager
                .Setup(x => x.RoleExistsAsync(model.RoleName))
                .ReturnsAsync(true);

            // Act
            var result = await service.CreateRoleAsync(model);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Role_Already_Exist", result.ErrorCode);

            mockRoleManager.Verify(x => x.RoleExistsAsync(model.RoleName), Times.Once);
            mockRoleManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Fact]
        public async Task CreateRole_ShouldFail_WhenCreateRoleFails()
        {
            // Arrange
            var model = new CreateRoleDto
            {
                RoleName = "Admin",
                Description = "Administrator"
            };

            mockRoleManager
                .Setup(x => x.RoleExistsAsync(model.RoleName))
                .ReturnsAsync(false);

            mockRoleManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Create role failed"
                        }));

            // Act
            var result = await service.CreateRoleAsync(model);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Add_Role_Failed", result.ErrorCode);
            Assert.Contains("Create role failed", result.Errors);

            mockRoleManager.Verify(x => x.RoleExistsAsync(model.RoleName), Times.Once);
            mockRoleManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationRole>()), Times.Once);
        }

        [Fact]
        public async Task CreateRole_ShouldSucceed()
        {
            // Arrange
            var model = new CreateRoleDto
            {
                RoleName = "Admin",
                Description = "Administrator"
            };

            mockRoleManager
                .Setup(x => x.RoleExistsAsync(model.RoleName))
                .ReturnsAsync(false);

            mockRoleManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await service.CreateRoleAsync(model);

            // Assert
            Assert.True(result.Success);

            mockRoleManager.Verify(x => x.RoleExistsAsync(model.RoleName), Times.Once);
            mockRoleManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationRole>()), Times.Once);
        }
    }
}