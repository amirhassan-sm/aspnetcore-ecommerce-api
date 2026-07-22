using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Category
{
    public class GetCategoryTests
    {
        private readonly Mock<ICategoryRepository> mockRepo;
        private readonly CategoryApplication service;
        public GetCategoryTests()
        {
            mockRepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mockRepo.Object);
        }

        [Fact]
        public async Task Get_ShouldFail_WhenCategoryDoesNotExist() {
            var categoryId = 1;

            mockRepo.Setup(x => x.Get(categoryId)).ReturnsAsync((Catalogue.Domain.Models.Category)null);

            var result = await service.Get(categoryId);


            Assert.False(result.Success);
            Assert.Equal("Category_Not_Exist",result.ErrorCode);

            mockRepo.Verify(x => x.Get(categoryId), Times.Once);
            






        }
        [Fact]
        public async Task Get_Should_Succeed()
        {
            var categoryId = 1;

            var category = new Catalogue.Domain.Models.Category
            {
                CategoryId = 1,
                CategoryName = "Phones",
                CategoryDescription = "Desc",
                ParentId = 2,
                Depth = 3,
                LineAge = "1,2,"
            };

            mockRepo.Setup(x => x.Get(categoryId))
                    .ReturnsAsync(category);
           

            var result = await service.Get(categoryId);


            Assert.True(result.Success);

            var model = result.Item[0];

            Assert.Equal(category.CategoryId, model.CategoryId);
            Assert.Equal(category.CategoryName, model.CategoryName);
            Assert.Equal(category.CategoryDescription, model.CategoryDescription);
            Assert.Equal(category.ParentId, model.ParentId);
            Assert.Equal(category.Depth, model.Depth);
            Assert.Equal(category.LineAge, model.LineAge);








        }
        [Fact]
        public async Task Get_ShouldFail_WhenExceptionThrows()
        {
            var categoryId = 1;

            mockRepo.Setup(x => x.Get(categoryId)).Throws(new Exception());

            var result = await service.Get(categoryId);


            Assert.False(result.Success);
            Assert.Equal("Execption_Occured", result.ErrorCode);

            mockRepo.Verify(x => x.Get(categoryId), Times.Once);







        }

    }
}
