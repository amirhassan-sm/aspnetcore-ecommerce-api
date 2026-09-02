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
    public class UpdateTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;
        private readonly Mock<ProductMediaService> mockMedia;


        public UpdateTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object, mockLogger.Object,mockMedia.Object);
        }

        [Fact]

        public async Task Update_ShouldFail_WhenProductDoesNotExist() {
            var model = new ProductUpdateModel {
            
            ProductId=1
            
            };

            mockRepo.Setup(x => x.IsPrductExist(model.ProductId)).ReturnsAsync(false);


            var result = await service.Update(model);


            Assert.False(result.Success);

            Assert.Equal("Product_Not_Exist",result.ErrorCode);


            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);

            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Never);

        }


        [Fact]

        public async Task Update_ShouldFail_WhenCategoryIsNotLevel3()
        {
            var model = new ProductUpdateModel
            {

                ProductId = 1,
                CategoryId=2

            };

            mockRepo.Setup(x => x.IsPrductExist(model.ProductId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(model.CategoryId)).ReturnsAsync(false);


            var result = await service.Update(model);


            Assert.False(result.Success);

            Assert.Equal("Category_Not_Level3", result.ErrorCode);


            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);

            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentNameAsync(It.IsAny<int>(),It.IsAny<string>()), Times.Never);

        }


        [Fact]

        public async Task Update_ShouldFail_WhenProductNameAlreadyExist()
        {
            var model = new ProductUpdateModel
            {

                ProductId = 1,
                CategoryId = 2,
                ProductName="Test",

            };

            mockRepo.Setup(x => x.IsPrductExist(model.ProductId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(model.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ProductExistExceptCurrentNameAsync(model.ProductId,model.ProductName)).ReturnsAsync(true);


            var result = await service.Update(model);


            Assert.False(result.Success);

            Assert.Equal("Product_Name_AlreadyExist", result.ErrorCode);


            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);

            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentNameAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);

        }

        [Fact]

        public async Task Update_ShouldFail_WhenProductSlugAlreadyExist()
        {
            var model = new ProductUpdateModel
            {

                ProductId = 1,
                CategoryId = 2,
                ProductName = "Test",
                Slug="testSlug"

            };

            mockRepo.Setup(x => x.IsPrductExist(model.ProductId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(model.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ProductExistExceptCurrentNameAsync(model.ProductId, model.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.ProductExistExceptCurrentSlugAsync(model.ProductId, model.Slug)).ReturnsAsync(true);


            var result = await service.Update(model);


            Assert.False(result.Success);

            Assert.Equal("Product_Slug_AlreadyExist", result.ErrorCode);


            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);

            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentNameAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            

        }

        [Fact]

        public async Task Update_ShouldSucceed()
        {
            var model = new ProductUpdateModel
            {

                ProductId = 1,
                CategoryId = 2,
                ProductName = "Test",
                Slug = "testSlug"

            };

            mockRepo.Setup(x => x.IsPrductExist(model.ProductId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(model.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ProductExistExceptCurrentNameAsync(model.ProductId, model.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.ProductExistExceptCurrentSlugAsync(model.ProductId, model.Slug)).ReturnsAsync(false);
            mockRepo.Setup(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>())).ReturnsAsync(true);


            var result = await service.Update(model);


            Assert.True(result.Success);



            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Once);

            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentNameAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);


        }

        [Fact]

        public async Task Update_ShouldFail_WhenExceptionThrows()
        {
            var model = new ProductUpdateModel
            {

                ProductId = 1,
                CategoryId = 2,
                ProductName = "Test",
                Slug = "testSlug"

            };

            mockRepo.Setup(x => x.IsPrductExist(model.ProductId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(model.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ProductExistExceptCurrentNameAsync(model.ProductId, model.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.ProductExistExceptCurrentSlugAsync(model.ProductId, model.Slug)).ReturnsAsync(false);
            mockRepo.Setup(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>())).Throws(new Exception());


            var result = await service.Update(model);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);



            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Once);

            mockRepo.Verify(x => x.IsPrductExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentNameAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ProductExistExceptCurrentSlugAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);


        }



    }
}
