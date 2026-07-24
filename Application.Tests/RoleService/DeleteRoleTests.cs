using Application.implementation;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Security.Identity;
using Xunit;

namespace Application.Tests.RoleService
{
    public class DeleteRoleTests
    {
        private readonly Mock<RoleManager<ApplicationRole>> mockRoleManager;
        private readonly RoleServices service;

        public DeleteRoleTests()
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
        public async Task DeleteRole_ShouldFail_WhenRoleDoesNotExist()
        {
            // Arrange
            var id = 1;

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync((ApplicationRole)null);

            // Act
            var result = await service.DeleteRoleAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Role_Not_Exist", result.ErrorCode);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Fact]
        public async Task DeleteRole_ShouldFail_WhenDeleteFails()
        {
            // Arrange
            var id = 1;

            var role = new ApplicationRole
            {
                Id = id,
                Name = "Admin"
            };

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync(role);

            mockRoleManager
                .Setup(x => x.DeleteAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Delete role failed"
                        }));

            // Act
            var result = await service.DeleteRoleAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Delete_Role_Failed", result.ErrorCode);
            Assert.Contains("Delete role failed", result.Errors);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRole_ShouldSucceed()
        {
            // Arrange
            var id = 1;

            var role = new ApplicationRole
            {
                Id = id,
                Name = "Admin"
            };

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync(role);

            mockRoleManager
                .Setup(x => x.DeleteAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await service.DeleteRoleAsync(id);

            // Assert
            Assert.True(result.Success);
            //Assert.Equal(id, result.Data);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRole_ShouldFail_WhenExceptionThrows()
        {
            // Arrange
            var id = 1;

            mockRoleManager
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ThrowsAsync(new Exception("Unexpected exception"));

            // Act
            var result = await service.DeleteRoleAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Exception_Occured", result.ErrorCode);

            mockRoleManager.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            mockRoleManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }
    }
}