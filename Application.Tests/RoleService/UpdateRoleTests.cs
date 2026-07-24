using Application.DTOs.Security.Roles;
using Application.implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Security.Identity;

namespace Application.Tests.RoleService
{
    public class UpdateRoleTests
    {
        private readonly Mock<RoleManager<ApplicationRole>> mockRoleManager;
        private readonly RoleServices service;

        public UpdateRoleTests()
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
        public async Task UpdateRole_ShouldFail_WhenRoleDoesNotExist()
        {
            // Arrange
            var id = 1;

            var model = new CreateRoleDto
            {
                RoleName = "Admin",
                Description = "Administrator"
            };

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync((ApplicationRole)null);

            // Act
            var result = await service.UpdateRolesAsync(model, id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Role_Not_Exist", result.ErrorCode);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Fact]
        public async Task UpdateRole_ShouldFail_WhenUpdateFails()
        {
            // Arrange
            var id = 1;

            var role = new ApplicationRole
            {
                Id = id,
                Name = "Old Role",
                Description = "Old Description"
            };

            var model = new CreateRoleDto
            {
                RoleName = "New Role",
                Description = "New Description"
            };

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync(role);

            mockRoleManager
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Update failed"
                        }));

            // Act
            var result = await service.UpdateRolesAsync(model, id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Update_Role_Failed", result.ErrorCode);
            Assert.Contains("Update failed", result.Errors);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationRole>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRole_ShouldSucceed()
        {
            // Arrange
            var id = 1;

            var role = new ApplicationRole
            {
                Id = id,
                Name = "Old Role",
                Description = "Old Description"
            };

            var model = new CreateRoleDto
            {
                RoleName = "New Role",
                Description = "New Description"
            };

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync(role);

            mockRoleManager
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await service.UpdateRolesAsync(model, id);

            // Assert
            Assert.True(result.Success);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationRole>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRole_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;

            var model = new CreateRoleDto
            {
                RoleName = "Admin",
                Description = "Administrator"
            };

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await service.UpdateRolesAsync(model, id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Exception_Occured", result.ErrorCode);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }
    }
}