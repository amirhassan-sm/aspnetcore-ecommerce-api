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
    public class RemoveProductTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;
        private readonly Mock<ProductMediaService> mockMedia;


        public RemoveProductTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object,mockMedia.Object);
        }
        [Fact]
        public async Task Remove_shouldFail_WhenPrductIsNotExist() {
            var productId = 1;

            mockRepo.Setup(x=>x.IsPrductExist(productId)).ReturnsAsync(false);



            var result = await service.Remove(productId);


            Assert.False(result.Success);

            Assert.Equal("Product_Not_Exist",result.ErrorCode);

            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.Remove(productId), Times.Never);
            mockRepo.Verify(x => x.RemoveAllFeaturesFromProduct(productId), Times.Never);





        }

        [Fact]
        public async Task Remove_shouldSuceed()
        {
            var productId = 1;

            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.RemoveAllFeaturesFromProduct(productId)).Returns(Task.CompletedTask);
            mockRepo.Setup(x => x.Remove(productId)).ReturnsAsync(true);



            var result = await service.Remove(productId);


            Assert.True(result.Success);


            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.Remove(productId), Times.Once);
            mockRepo.Verify(x => x.RemoveAllFeaturesFromProduct(productId), Times.Once);





        }

        [Fact]
        public async Task Remove_shouldFail_WhenExceptionThrows()
        {
            var productId = 1;

            mockRepo.Setup(x => x.IsPrductExist(productId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.RemoveAllFeaturesFromProduct(productId)).Returns(Task.CompletedTask);
            mockRepo.Setup(x => x.Remove(productId)).ThrowsAsync(new Exception { });



            var result = await service.Remove(productId);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);




            mockRepo.Verify(x => x.IsPrductExist(productId), Times.Once);

            mockRepo.Verify(x => x.Remove(productId), Times.Once);
            mockRepo.Verify(x => x.RemoveAllFeaturesFromProduct(productId), Times.Once);





        }
    }
}
