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
    public class FeatureUpdateTests
    {
        private readonly Mock<IFeatureRepository> mockRepo;
        private readonly Mock<IFeatureQueryServices> mockQuery;
        private readonly FeatureApplication service;
        private readonly Mock<ILogger<FeatureApplication>> mockLoger;

        public FeatureUpdateTests()
        {
            mockRepo = new Mock<IFeatureRepository>();
            mockQuery = new Mock<IFeatureQueryServices>();
            mockLoger = new Mock<ILogger<FeatureApplication>>();
            service = new FeatureApplication(mockRepo.Object, mockQuery.Object, mockLoger.Object);

        }
        [Fact]
        public async Task Update_ShouldFail_WhenFeatureIsNotExist()
        {
            var model = new FeatureModel
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"


            };
            var featuer = new Catalogue.Domain.Models.Feature
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"



            };



            mockRepo.Setup(x => x.IsFeatureExist(model.FeatureId)).ReturnsAsync(false);

            var result = await service.Update(model);

            Assert.False(result.Success);
            Assert.Equal("Featuer_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Never());
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once());
            mockRepo.Verify(x => x.IsFeatureNameExistExceptCurrent(It.IsAny<int>(), It.IsAny<string>()), Times.Never());

        }
        [Fact]
        public async Task Update_ShouldFail_WhenFeatureNameAlreadyExist()
        {
            var model = new FeatureModel
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"


            };
            var featuer = new Catalogue.Domain.Models.Feature
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"



            };



            mockRepo.Setup(x => x.IsFeatureExist(model.FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureNameExistExceptCurrent(model.FeatureId, model.FeatureName)).ReturnsAsync(true);

            var result = await service.Update(model);

            Assert.False(result.Success);
            Assert.Equal("Featuer_Already_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Never());
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once());
            mockRepo.Verify(x => x.IsFeatureNameExistExceptCurrent(It.IsAny<int>(), It.IsAny<string>()), Times.Once());

        }

        [Fact]
        public async Task Update_ShouldSucceed()
        {
            var model = new FeatureModel
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"


            };
            var featuer = new Catalogue.Domain.Models.Feature
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"



            };



            mockRepo.Setup(x => x.IsFeatureExist(model.FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureNameExistExceptCurrent(model.FeatureId, model.FeatureName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.Update(featuer)).Returns(Task.CompletedTask);

            var result = await service.Update(model);

            Assert.True(result.Success);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Once());
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once());
            mockRepo.Verify(x => x.IsFeatureNameExistExceptCurrent(It.IsAny<int>(), It.IsAny<string>()), Times.Once());



        }

        [Fact]
        public async Task Update_ShouldFail_whenExceptionThrows()
        {
            var model = new FeatureModel
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"


            };
            var featuer = new Catalogue.Domain.Models.Feature
            {
                FeatureDescription = "testDescription",
                FeatureId = 1,
                FeatureName = "test"



            };



            mockRepo.Setup(x => x.IsFeatureExist(model.FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureNameExistExceptCurrent(model.FeatureId, model.FeatureName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.Update(It.IsAny<Catalogue.Domain.Models.Feature>()))
      .ThrowsAsync(new Exception());

            var result = await service.Update(model);

            Assert.False(result.Success);
            Assert.Equal("Excpetion_Occured", result.ErrorCode);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Feature>()), Times.Once());
            mockRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once());
            mockRepo.Verify(x => x.IsFeatureNameExistExceptCurrent(It.IsAny<int>(), It.IsAny<string>()), Times.Once());



        }
    }
}
