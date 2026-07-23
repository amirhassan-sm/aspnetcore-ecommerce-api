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
    public class GetAllProductFeaturesTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;

        public GetAllProductFeaturesTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object);
        }
        [Fact]

        public async Task GetAllProductFeatures_ShouldFail_WhenProductIsNotExist()
        {
            var productId = 1;
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(false);


            var result = await service.GetAllProductFeatures(productId);



            Assert.False(result.Success);

            Assert.Equal("Product_Not_Exist",result.ErrorCode);
            Assert.Null(result.Item);

            mockRepo.Verify(x => x.GetAllProductFeatures(It.IsAny<int>()), Times.Never);
            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);






        }

        [Fact]

        public async Task GetAllProductFeatures_ShouldSucceed()
        {
            var productId = 1;
            var features = new List<Catalogue.Domain.Models.ProductFeature> { 
            new Catalogue.Domain.Models.ProductFeature{ FeatureId=1,Value="test"}



            };
            var model = new List<ProductFeaturesModel> {
              new ProductFeaturesModel{
                FeatureId = 1,
                Value = "testValue",
                FeatureName = "testFaeture",
                ProductFeatureId = 1,
                ProductId=1


              },






            };
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.GetAllProductFeatures(productId)).ReturnsAsync(features);


            mockQuery.Setup(x => x.GetProductFeatureModels(features)).Returns(model);


            var result = await service.GetAllProductFeatures(productId);
            var items = result.Item.SingleOrDefault();



            Assert.True(result.Success);

            Assert.Equal(items.Value, "testValue");
            Assert.Equal(items.FeatureId, 1);
            Assert.Equal(items.FeatureName, "testFaeture");
            Assert.Equal(items.ProductFeatureId, 1);
            Assert.Equal(items.ProductId, 1);


            mockRepo.Verify(x => x.GetAllProductFeatures(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockQuery.Verify(x => x.GetProductFeatureModels(It.IsAny<List<Catalogue.Domain.Models.ProductFeature>>()), Times.Once);







        }

        [Fact]

        public async Task GetAllProductFeatures_ShouldFail_WhenExceptionThrows()
        {
            var productId = 1;
            var features = new List<Catalogue.Domain.Models.ProductFeature> {
            new Catalogue.Domain.Models.ProductFeature{ FeatureId=1,Value="test"}



            };

          
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.GetAllProductFeatures(productId)).Throws(new Exception { });



            var result = await service.GetAllProductFeatures(productId);




            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);




            mockRepo.Verify(x => x.GetAllProductFeatures(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockQuery.Verify(x => x.GetProductFeatureModels(It.IsAny<List<Catalogue.Domain.Models.ProductFeature>>()), Times.Never);








        }
    }
}
