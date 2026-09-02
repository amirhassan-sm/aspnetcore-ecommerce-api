using Application.Contrast.QueryServices;
using Application.DTOs.Product;
using Application.implementation;
using Catalogue.Domain.Models;
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
    public class AddNewProductTests
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductQueryServices> mockQuery;
        private readonly Mock<ILogger<ProductApplication>> mockLogger;
        private readonly ProductApplication service;
        private readonly Mock<ProductMediaService> mockMedia;

        public AddNewProductTests()
        {
            mockRepo = new Mock<IProductRepository>();
            mockQuery = new Mock<IProductQueryServices>();
            mockLogger = new Mock<ILogger<ProductApplication>>();
            service = new ProductApplication(mockRepo.Object, mockQuery.Object,mockLogger.Object,mockMedia.Object);
        }
        [Fact]

        public async Task Add_ShouldFail_WhenNameAlreadyExist()
        {
            var prod = new ProductAddModel {
            ProductName = "Test"
            
            
            };
            mockRepo.Setup(x => x.ExistProductNameAsync(prod.ProductName)).ReturnsAsync(true);



            var result = await service.AddNew(prod);



            Assert.False(result.Success);
            Assert.Equal("Product_Already_Exist",result.ErrorCode);


            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);
            mockRepo.Verify(x => x.ExistProductNameAsync(It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Never);

        }
        [Fact]
        public async Task Add_ShouldFail_WhenCategoryDoesNotExist()
        {
            var prod = new ProductAddModel
            {
                ProductName = "Test",
                CategoryId=1


            };
            mockRepo.Setup(x => x.ExistProductNameAsync(prod.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.IsCategoryExist(prod.CategoryId)).ReturnsAsync(false);



            var result = await service.AddNew(prod);



            Assert.False(result.Success);
            Assert.Equal("Category_Not_Exist", result.ErrorCode);


            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);
            mockRepo.Verify(x => x.ExistProductNameAsync(It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.IsAny<int>()), Times.Never);

        }
        [Fact]
        public async Task Add_ShouldFail_WhenCategoryIsNotLevel3()
        {
            var prod = new ProductAddModel
            {
                ProductName = "Test",
                CategoryId = 1


            };
            mockRepo.Setup(x => x.ExistProductNameAsync(prod.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.IsCategoryExist(prod.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(prod.CategoryId)).ReturnsAsync(false);



            var result = await service.AddNew(prod);



            Assert.False(result.Success);
            Assert.Equal("Category_IsNot_level3", result.ErrorCode);


            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);
            mockRepo.Verify(x => x.ExistProductNameAsync(It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.Is<int>(x=>x == prod.CategoryId)), Times.Once);

        }
        [Fact]
        public async Task Add_ShouldFail_WhenPriceIsNegetive()
        {
            var prod = new ProductAddModel
            {
                ProductName = "Test",
                CategoryId = 1,
                BasePrice = -1,


            };
            mockRepo.Setup(x => x.ExistProductNameAsync(prod.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.IsCategoryExist(prod.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(prod.CategoryId)).ReturnsAsync(true);



            var result = await service.AddNew(prod);



            Assert.False(result.Success);
            Assert.Equal("Price_Is_negetive", result.ErrorCode);


            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Never);
            mockRepo.Verify(x => x.ExistProductNameAsync(It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.Is<int>(x => x == prod.CategoryId)), Times.Once);

        }

        [Fact]
        public async Task Add_ShouldSucceed()
        {
            var prod = new ProductAddModel
            {
                ProductName = "Test",
                CategoryId = 1,
                BasePrice = 1000000,


            };
       
            var catFeatures = new List<int> { 1, 2 };

            mockRepo.Setup(x => x.ExistProductNameAsync(prod.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.IsCategoryExist(prod.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(prod.CategoryId)).ReturnsAsync(true);



            mockRepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>())).ReturnsAsync(1);

            mockRepo.Setup(x => x.GetProductCategoryFeatures(1)).ReturnsAsync(catFeatures);
            mockRepo.Setup(x => x.AsignAllCategoryFeaturesToProduct(1, catFeatures)).Returns(Task.CompletedTask);




            var result = await service.AddNew(prod);



            Assert.True(result.Success);


            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Once);
            mockRepo.Verify(x => x.ExistProductNameAsync(It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.Is<int>(x => x == prod.CategoryId)), Times.Once);
            mockRepo.Verify(x => x.GetProductCategoryFeatures(1), Times.Once);
            mockRepo.Verify(x => x.AsignAllCategoryFeaturesToProduct(1,catFeatures), Times.Once);



        }
        [Fact]
        public async Task Add_Fail_WhenExceptionThrows()
        {
            var prod = new ProductAddModel
            {
                ProductName = "Test",
                CategoryId = 1,
                BasePrice = 1000000,


            };
            
            var catFeatures = new List<int> { 1, 2 };

            mockRepo.Setup(x => x.ExistProductNameAsync(prod.ProductName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.IsCategoryExist(prod.CategoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsCategoryLevel3(prod.CategoryId)).ReturnsAsync(true);



            mockRepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>())).Throws(new Exception {HelpLink="xxx"});

            mockRepo.Setup(x => x.GetProductCategoryFeatures(1)).ReturnsAsync(catFeatures);
            mockRepo.Setup(x => x.AsignAllCategoryFeaturesToProduct(1, catFeatures)).Returns(Task.CompletedTask);




            var result = await service.AddNew(prod);



            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);


            mockRepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Product>()), Times.Once);
            mockRepo.Verify(x => x.ExistProductNameAsync(It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(x => x.IsCategoryLevel3(It.Is<int>(x => x == prod.CategoryId)), Times.Once);
            mockRepo.Verify(x => x.GetProductCategoryFeatures(1), Times.Never);
            mockRepo.Verify(x => x.AsignAllCategoryFeaturesToProduct(1, catFeatures), Times.Never);

            

        }








    }
}
