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

namespace Application.Tests.Product
{
    public class RemoveFeaturesFromProductTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;
        private readonly Mock<ProductMediaService> mockMedia;


        public RemoveFeaturesFromProductTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object,mockMedia.Object);
        }
        [Fact]

        public async Task RemoveFeatureFromProduct_ShouldFail_WhenFeatureIsNotExist()
        {
            var featuerId = 1;
            var productId = 2;


            mockRepo.Setup(x => x.IsFeaureExistAsync(featuerId)).ReturnsAsync(false);



            var result = await service.RemoveFeaturesFromProductAsync(featuerId, productId);


            Assert.False(result.Success);

            Assert.Equal("Feature_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.IsFeaureExistAsync(featuerId), Times.Once);
            mockRepo.Verify(x => x.RemoveFeaturesFromProductAsync(featuerId,productId), Times.Never);
            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Never);





        }

        [Fact]

        public async Task RemoveFeatureFromProduct_ShouldFail_WhenProductIsNotExist()
        {
            var featuerId = 1;
            var productId = 2;

            mockRepo.Setup(x => x.IsFeaureExistAsync(featuerId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(false);



            var result = await service.RemoveFeaturesFromProductAsync(featuerId, productId);



            Assert.False(result.Success);

            Assert.Equal("Product_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeaturesFromProductAsync(featuerId,productId), Times.Never);

            mockRepo.Verify(x => x.IsFeaureExistAsync(featuerId), Times.Once);

            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.IsProductFeatureExistAsync(productId, featuerId), Times.Never);





        }

        [Fact]

        public async Task RemoveFeatureFromProduct_ShouldFail_WhenFeatureDoesNotAssignedToProduct()
        {
            var featuerId = 1;
            var productId = 2;

            mockRepo.Setup(x => x.IsFeaureExistAsync(featuerId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsProductFeatureExistAsync(productId, featuerId)).ReturnsAsync(false)
                ;



            var result = await service.RemoveFeaturesFromProductAsync(featuerId,productId);



            Assert.False(result.Success);

            Assert.Equal("Feature_DoesNot_Assigned", result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeaturesFromProductAsync(featuerId,productId), Times.Never);

            mockRepo.Verify(x => x.IsFeaureExistAsync(featuerId), Times.Once);

            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.IsProductFeatureExistAsync(productId, featuerId), Times.Once);





        }

        [Fact]

        public async Task RemoveFeatureFromProduct_ShouldSuceed()
        {
            var featuerId = 1;
            var productId = 2;

            mockRepo.Setup(x => x.IsFeaureExistAsync(featuerId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsProductFeatureExistAsync(productId, featuerId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.RemoveFeaturesFromProductAsync(featuerId, productId)).Returns(Task.CompletedTask);




            var result = await service.RemoveFeaturesFromProductAsync(featuerId, productId);



            Assert.True(result.Success);


            mockRepo.Verify(x => x.RemoveFeaturesFromProductAsync(featuerId, productId), Times.Once);

            mockRepo.Verify(x => x.IsFeaureExistAsync(featuerId), Times.Once);

            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.IsProductFeatureExistAsync(productId, featuerId), Times.Once);





        }

        [Fact]

        public async Task RemoveFeatureFromProduct_ShouldFail_WhenExceptionThrows()
        {
            var featuerId = 1;
            var productId = 2;

            mockRepo.Setup(x => x.IsFeaureExistAsync(featuerId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsProductFeatureExistAsync(productId, featuerId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.RemoveFeaturesFromProductAsync(featuerId, productId)).Throws(new Exception());



            var result = await service.RemoveFeaturesFromProductAsync(featuerId, productId);



            Assert.False(result.Success);

            Assert.Equal("Exception_Occured", result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeaturesFromProductAsync(featuerId, productId), Times.Once);


            mockRepo.Verify(x => x.IsFeaureExistAsync(featuerId), Times.Once);

            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.IsProductFeatureExistAsync(productId, featuerId), Times.Once);





        }
    }
}
