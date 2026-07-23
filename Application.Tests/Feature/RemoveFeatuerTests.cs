using Application.Contrast.QueryServices;
using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Runner.Common;

namespace Application.Tests.Feature
{

    public  class RemoveFeatuerTests
    {
        private readonly Mock<IFeatureRepository> mockRepo;
        private readonly Mock<IFeatureQueryServices> mockQuery;
        private readonly FeatureApplication service;
        private readonly Mock<ILogger<FeatureApplication>> mockLoger;

        public RemoveFeatuerTests()
        {
            mockRepo = new Mock<IFeatureRepository>();
            mockQuery = new Mock<IFeatureQueryServices>();
            mockLoger = new Mock<ILogger<FeatureApplication>>();
            service = new FeatureApplication(mockRepo.Object, mockQuery.Object, mockLoger.Object);

        }
        [Fact]

        public async Task Remove_ShouldFail_WhenFeatureNotExist()
        {
            var FeatureId = 1;

            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(false);

            var result = await service.Remove(FeatureId);

            Assert.False(result.Success);

            Assert.Equal("Feature_Not_Exists",result.ErrorCode);

            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Never);
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.HasFeatureAssigned(It.IsAny<int>()), Times.Never);



        }
        [Fact]

        public async Task Remove_ShouldFail_WhenFeatuerAssigned()
        {
            var FeatureId = 1;

            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.HasFeatureAssigned(FeatureId)).ReturnsAsync(true);

            var result = await service.Remove(FeatureId);

            Assert.False(result.Success);

            Assert.Equal("Feature_Assigned", result.ErrorCode);

            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Never);
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.HasFeatureAssigned(It.IsAny<int>()), Times.Once);
      



        }

        [Fact]

        public async Task Remove_ShouldSucceed()
        {
            var FeatureId = 1;

            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.HasFeatureAssigned(FeatureId)).ReturnsAsync(false);
            mockRepo.Setup(x => x.Remove(FeatureId)).Returns(Task.CompletedTask);

            var result = await service.Remove(FeatureId);

            Assert.True(result.Success);

      

            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.HasFeatureAssigned(It.IsAny<int>()), Times.Once);




        }

        [Fact]

        public async Task Remove_ShouldFail_WhenExceptionOccured()
        {
            var FeatureId = 1;

            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.HasFeatureAssigned(FeatureId)).ReturnsAsync(false);
            mockRepo.Setup(x => x.Remove(FeatureId)).ThrowsAsync(new Exception());

            var result = await service.Remove(FeatureId);

            Assert.False(result.Success);
            Assert.Equal("Exception_Occured", result.ErrorCode);



            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.HasFeatureAssigned(It.IsAny<int>()), Times.Once);




        }

    }
}
