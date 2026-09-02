using Application.Contrast.QueryServices;
using Application.DTOs.Product;
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
    public class SetSeoModelTests
    {

        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;
        private readonly Mock<ProductMediaService> mockMedia;


        public SetSeoModelTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object,mockMedia.Object);
        }
        [Fact]
        public async Task SetSeo_ShouldFail_WhenProductDoesNotExist()
        {
            var seoModel = new ProductSeoModel
            {
                ProductId = 1,
                Slug = "test"

            };

            mockRepo.Setup(x=>x.IsPrductExist(seoModel.ProductId)).ReturnsAsync(false);

            var result = await service.setSeoAsync(seoModel);


            Assert.False(result.Success);

            Assert.Equal("Product_Not_Exist",result.ErrorCode);

            mockRepo.Verify(x => x.IsPrductExist(seoModel.ProductId), Times.Once);
            mockRepo.Verify(x => x.setSeoAsync(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(It.IsAny<int>(),It.IsAny<string>()), Times.Never);





        }
        [Fact]
        public async Task SetSeo_ShouldFail_WhenSlugAlreadyExist()
        {
            var seoModel = new ProductSeoModel
            {
                ProductId = 1,
                Slug = "test"

            };

            mockRepo.Setup(x => x.IsPrductExist(seoModel.ProductId)).ReturnsAsync(true);

            mockRepo.Setup(x => x.ProductExistExceptCurrentSlugAsync(seoModel.ProductId,seoModel.Slug)).ReturnsAsync(true);

            var result = await service.setSeoAsync(seoModel);


            Assert.False(result.Success);

            Assert.Equal("Slug_Already_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.IsPrductExist(seoModel.ProductId), Times.Once);
            mockRepo.Verify(x => x.setSeoAsync(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(seoModel.ProductId, seoModel.Slug), Times.Once);






        }

        [Fact]
        public async Task SetSeo_ShouldSuceed()
        {
            var seoModel = new ProductSeoModel
            {
                ProductId = 1,
                Slug = "test"

            };


            mockRepo.Setup(x => x.IsPrductExist(seoModel.ProductId)).ReturnsAsync(true);

            mockRepo.Setup(x => x.ProductExistExceptCurrentSlugAsync(seoModel.ProductId, seoModel.Slug)).ReturnsAsync(false);
            mockRepo.Setup(x => x.setSeoAsync(It.IsAny<Catalogue.Domain.Models.Product>())).Returns(Task.CompletedTask);




            var result = await service.setSeoAsync(seoModel);


            Assert.True(result.Success);


            mockRepo.Verify(x => x.IsPrductExist(seoModel.ProductId), Times.Once);
            mockRepo.Verify(x => x.setSeoAsync(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(seoModel.ProductId, seoModel.Slug), Times.Once);






        }
        [Fact]
        public async Task SetSeo_Shouldfail_WhenExceptionThrows()
        {
            var seoModel = new ProductSeoModel
            {
                ProductId = 1,
                Slug = "test"

            };


            mockRepo.Setup(x => x.IsPrductExist(seoModel.ProductId)).ReturnsAsync(true);

            mockRepo.Setup(x => x.ProductExistExceptCurrentSlugAsync(seoModel.ProductId, seoModel.Slug)).ReturnsAsync(false);
            mockRepo.Setup(x => x.setSeoAsync(It.IsAny<Catalogue.Domain.Models.Product>())).Throws(new Exception());




            var result = await service.setSeoAsync(seoModel);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);


            mockRepo.Verify(x => x.IsPrductExist(seoModel.ProductId), Times.Once);
            mockRepo.Verify(x => x.setSeoAsync(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(seoModel.ProductId, seoModel.Slug), Times.Once);






        }
    }
}
