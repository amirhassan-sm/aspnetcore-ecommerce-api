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
    public class GetCategoryTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;
        private readonly Mock<ProductMediaService> mockMedia;


        public GetCategoryTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object,mockMedia.Object);
        }
        [Fact]

        public async Task Get_ShouldFail_WhenResposeIsNull() 
        {
            var productId = 1;

            mockRepo.Setup(x=>x.Get(productId)).ReturnsAsync((Catalogue.Domain.Models.Product)null);


            var result = await service.Get(productId);


            Assert.False(result.Success);


            Assert.Equal("Product_Not_exis",result.ErrorCode);

            mockRepo.Verify(x=>x.Get(productId),Times.Once);

            Assert.Null(result.Item);

        }

        [Fact]

        public async Task Get_ShouldSuceed()
        {
            var productId = 1;

            var product = new Catalogue.Domain.Models.Product
            {
                ProductId = 1,
                ProductName = "Laptop",
                CategoryId = 5,
                BasePrice = 1200,
                Slug = "laptop",
                //ImageUrl = "img.jpg"
            };

            mockRepo.Setup(x => x.Get(productId)).ReturnsAsync(product);


            var result = await service.Get(productId);


            Assert.True(result.Success);



            mockRepo.Verify(x => x.Get(productId), Times.Once);

            Assert.Equal( product.ProductName, result.Item.ProductName);
            Assert.Equal(product.ProductId, result.Item.ProductId);
            Assert.Equal(product.ProductName, result.Item.ProductName);
            Assert.Equal(product.CategoryId, result.Item.CategoryId);
            Assert.Equal(product.BasePrice, result.Item.BasePrice);
            Assert.Equal(product.Slug, result.Item.Slug);


        }
        [Fact]
        public async Task Get_ShouldFail_WhenExcptionthrows()
        {
            var productId = 1;


            mockRepo.Setup(x => x.Get(productId)).ThrowsAsync(new Exception { });


            var result = await service.Get(productId);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);
            Assert.Null(result.Item);



            mockRepo.Verify(x => x.Get(productId), Times.Once);



        }


    }
}
