using Application.Contrast.QueryServices;
using Application.DTOs.Feature;
using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Feature
{
    public class FeatureAddTests
    {
        private readonly Mock<IFeatureRepository> mockRepo;
        private readonly Mock<IFeatureQueryServices> mockQuery;
        private readonly FeatureApplication service;
        private readonly Mock<ILogger<FeatureApplication>> mockLoger;

        public FeatureAddTests()
        {
            mockRepo = new Mock<IFeatureRepository>();
            mockQuery = new Mock<IFeatureQueryServices>();
            mockLoger = new Mock<ILogger<FeatureApplication>>();
            service = new FeatureApplication(mockRepo.Object, mockQuery.Object,mockLoger.Object);
            
        }

        [Fact]
        public async Task AddNew_ShouldFaild_WhenFeatureNameAlreadyExist()
        { 
             var model = new FeatureAddModel
             {
                 FeatureDescription="test Descrption",
                 FeatureName="test"
                 
            
            };
            mockRepo.Setup(x => x.IsFeatureNameExist(model.FeatureName)).ReturnsAsync(true);

            var result = await service.AddNew(model);

            Assert.Equal("FeatureName_Already_exist", result.ErrorCode);
            Assert.False(result.Success);

            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Never);
            mockRepo.Verify(x => x.IsFeatureNameExist(It.IsAny<string>()), Times.Once);








        }

        [Fact]
        public async Task AddNew_ShouldSucceed()
        {
            var model = new FeatureAddModel
            {
                FeatureDescription = "test Descrption",
                FeatureName = "test"


            };
            mockRepo.Setup(x => x.IsFeatureNameExist(model.FeatureName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Feature>())).ReturnsAsync(1);

            var result = await service.AddNew(model);

            Assert.True(result.Success);

            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Once);
            mockRepo.Verify(x => x.IsFeatureNameExist(It.IsAny<string>()), Times.Once);








        }

        [Fact]
        public async Task AddNew_ShouldFail_WhenExceptionThrows()
        {
            var model = new FeatureAddModel
            {
                FeatureDescription = "test Descrption",
                FeatureName = "test"


            };
            mockRepo.Setup(x => x.IsFeatureNameExist(model.FeatureName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Feature>())).ThrowsAsync(new Exception());

            var result = await service.AddNew(model);

            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);

            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Once);
            mockRepo.Verify(x => x.IsFeatureNameExist(It.IsAny<string>()), Times.Once);








        }

    }
}