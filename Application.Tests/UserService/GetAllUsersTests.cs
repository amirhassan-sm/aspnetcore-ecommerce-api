using Application.Contrast.Repositories;
using Application.DTOs.Security;
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
    public class GetAllUsersTests
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<IUserManegment> mockRepo;
        private readonly UserServices service;

        public GetAllUsersTests()
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

        public async Task Get_should_Suceed()
        {


            mockRepo.Setup(x => x.GetAllUsersQuery()).ReturnsAsync(new List<UserProfileDto>());

            var result = await service.GetAllUsersAsync();

            Assert.True(result.Success);

            mockRepo.Verify(x => x.GetAllUsersQuery(), Times.Once);






        }

        [Fact]

        public async Task Get_shouldFail_WhenExceptionthrows()
        {


            mockRepo.Setup(x => x.GetAllUsersQuery()).ThrowsAsync(new Exception());

            var result = await service.GetAllUsersAsync();

            Assert.False(result.Success);
            Assert.Equal("Exception_Occured", result.ErrorCode);

            mockRepo.Verify(x => x.GetAllUsersQuery(), Times.Once);






        }

    }
}
