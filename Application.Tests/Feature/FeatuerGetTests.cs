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
    public class FeatuerGetTests
    {
        private readonly Mock<IFeatureRepository> mockRepo;
        private readonly Mock<IFeatureQueryServices> mockQuery;
        private readonly FeatureApplication service;
        private readonly Mock<ILogger<FeatureApplication>> mockLoger;

        public FeatuerGetTests()
        {
            mockRepo = new Mock<IFeatureRepository>();
            mockQuery = new Mock<IFeatureQueryServices>();
            mockLoger = new Mock<ILogger<FeatureApplication>>();
            service = new FeatureApplication(mockRepo.Object, mockQuery.Object, mockLoger.Object);

        }
        [Fact]
        public async Task Get_shouldFaild_WhenFeatureNotExist()
        {
            var featureID = 1;

            mockRepo.Setup(x => x.IsFeatureExist(featureID)).ReturnsAsync(false);

            var result = await service.Get(featureID);

            Assert.False(result.Success);
            Assert.Equal("Featuer_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.IsFeatureExist(featureID), Times.Once);


            mockRepo.Verify(x => x.Get(featureID), Times.Never);
        }

        [Fact]
        public async Task Get_ShouldSucceed()
        {
            var featureID = 1;
            var feature = new Catalogue.Domain.Models.Feature
            {
                FeatureId = 1,
                FeatureName = "test",
                FeatureDescription = "testDescription"


            };
            var model = new FeatureModel
            {
                FeatureId = 1,
                FeatureName = "test",
                FeatureDescription = "testDescription"





            };

            mockRepo.Setup(x => x.IsFeatureExist(featureID)).ReturnsAsync(true);
            mockRepo.Setup(x => x.Get(featureID)).ReturnsAsync(feature);

            var result = await service.Get(featureID);

            Assert.True(result.Success);

            Assert.Equal(result.Item.FeatureName, model.FeatureName);
            Assert.Equal(result.Item.FeatureId, model.FeatureId);
            Assert.Equal(result.Item.FeatureDescription, model.FeatureDescription);


            mockRepo.Verify(x => x.IsFeatureExist(featureID), Times.Once);
            mockRepo.Verify(x => x.Get(featureID), Times.Once);
        }


        [Fact]
        public async Task Get_ShouldFaild_ExceptionThrows()
        {
            var featureID = 1;
            var feature = new Catalogue.Domain.Models.Feature
            {
                FeatureId = 1,
                FeatureName = "test",
                FeatureDescription = "testDescription"


            };
            var model = new FeatureModel
            {
                FeatureId = 1,
                FeatureName = "test",
                FeatureDescription = "testDescription"





            };

            mockRepo.Setup(x => x.IsFeatureExist(featureID)).ReturnsAsync(true);
            mockRepo.Setup(x => x.Get(featureID)).ThrowsAsync(new Exception());

            var result = await service.Get(featureID);

            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);



            mockRepo.Verify(x => x.IsFeatureExist(featureID), Times.Once);
            mockRepo.Verify(x => x.Get(featureID), Times.Once);
        }
    }
}
