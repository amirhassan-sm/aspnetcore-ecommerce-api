using Application.Contrast.QueryServices;
using Application.DTOs.ProductFeatures;
using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Product
{
    public class SetFeatureValuesTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;

        public SetFeatureValuesTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object);
        }

        [Fact]

        public async Task SetFeatureValues_ShouldFail_WhenProductFeatureDoesNotExist() {
            var model =new SetProductFeatureModel {
            FeatureId=1,
            FeaureValue="test",
            ProductId=2
            
            };

            mockRepo.Setup(x => x.IsProductFeatureExistAsync(model.ProductId, model.FeatureId)).ReturnsAsync(false);


            var result = await service.SetFeatureValues(model);



            Assert.False(result.Success);

            Assert.Equal("ProductFeature_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.IsProductFeatureExistAsync(model.ProductId, model.FeatureId), Times.Once);
            mockRepo.Verify(x => x.SetFeatureValues(model.ProductId, model.FeatureId, model.FeaureValue), Times.Never);
            









        }

        [Fact]

        public async Task SetFeatureValues_ShouldSucceed()
        {
            var model = new SetProductFeatureModel
            {
                FeatureId = 1,
                FeaureValue = "test",
                ProductId = 2

            };

            mockRepo.Setup(x => x.IsProductFeatureExistAsync(model.ProductId, model.FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.SetFeatureValues(model.ProductId, model.FeatureId, model.FeaureValue)).Returns(Task.CompletedTask);


            var result = await service.SetFeatureValues(model);



            Assert.True(result.Success);


            mockRepo.Verify(x => x.IsProductFeatureExistAsync(model.ProductId, model.FeatureId), Times.Once);
            mockRepo.Verify(x => x.SetFeatureValues(model.ProductId, model.FeatureId, model.FeaureValue), Times.Once);










        }

        [Fact]

        public async Task SetFeatureValues_ShouldFail_WhenExceptionThrows()
        {
            var model = new SetProductFeatureModel
            {
                FeatureId = 1,
                FeaureValue = "test",
                ProductId = 2

            };

            mockRepo.Setup(x => x.IsProductFeatureExistAsync(model.ProductId, model.FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.SetFeatureValues(model.ProductId, model.FeatureId, model.FeaureValue)).Throws(new Exception());


            var result = await service.SetFeatureValues(model);



            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);


            mockRepo.Verify(x => x.IsProductFeatureExistAsync(model.ProductId, model.FeatureId), Times.Once);
            mockRepo.Verify(x => x.SetFeatureValues(model.ProductId, model.FeatureId, model.FeaureValue), Times.Once);










        }



    }
}
