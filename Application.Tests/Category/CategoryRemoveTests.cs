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
    public class CategoryRemoveTests
    {
        private readonly Mock<ICategoryRepository> mockRepo;
        private readonly CategoryApplication service;
        public CategoryRemoveTests()
        {
            mockRepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mockRepo.Object);


        }


        // remove
        [Fact]
        public async Task Remove_ShouldFail_WhenCategoryHasChild()
        {
            // arrange 
            var categoryId = 1;


            mockRepo.Setup(x => x.HasChildCategory(categoryId))
                .ReturnsAsync(true);


            //Act
            var result = await service.Remove(categoryId);

            //assert
            Assert.False(result.Success);
            Assert.Equal("Category_Has_Child", result.ErrorCode);
            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Never);

        }
        [Fact]
        public async Task Remove_ShouldFail_WhenCategoryHasProduct()
        {
            var categoryId = 1;

            mockRepo.Setup(x => x.HasChildCategory(categoryId)).ReturnsAsync(false);
            mockRepo.Setup(x => x.HasProductCategory(categoryId)).ReturnsAsync(true);

            var result = await service.Remove(categoryId);

            Assert.False(result.Success);
            Assert.Equal("Category_Has_Product", result.ErrorCode);
            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Never);

        }

        [Fact]
        public async Task Remove_ShouldFail_WhenCategoryNotExist()
        {

            var categoryId = 1;

            mockRepo.Setup(x => x.HasChildCategory(categoryId)).ReturnsAsync(false);
            mockRepo.Setup(x => x.HasProductCategory(categoryId)).ReturnsAsync(false);

            mockRepo.Setup(x => x.IsCategoryExist(categoryId))
                .ReturnsAsync(false);

            var result = await service.Remove(categoryId);



            Assert.False(result.Success);
            Assert.Equal("Category_DoesNot_Exist", result.ErrorCode);
            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Never);


        }
        [Fact]
        public async Task Remove_ShouldReturnSuccess_WhenCategoryRemoved()
        {
            var categoryId = 1;

            mockRepo.Setup(x => x.HasChildCategory(categoryId)).ReturnsAsync(false);
            mockRepo.Setup(x => x.HasProductCategory(categoryId)).ReturnsAsync(false);

            mockRepo.Setup(x => x.IsCategoryExist(categoryId))
                .ReturnsAsync(true);

            mockRepo.Setup(x => x.RemoveAllFeaturesFromCategory(categoryId)).Returns(Task.CompletedTask);

            mockRepo.Setup(x => x.Remove(categoryId)).ReturnsAsync(true);

            var result = await service.Remove(categoryId);

            Assert.True(result.Success);

            mockRepo.Verify(x => x.RemoveAllFeaturesFromCategory(categoryId), Times.Once);

            mockRepo.Verify(x => x.Remove(categoryId), Times.Once);



        }
        [Fact]
        public async Task Remove_ShouldFail_WhenRepoThrowAnException()
        {
            var categoryId = 1;
            mockRepo.Setup(x => x.HasChildCategory(categoryId)).ReturnsAsync(false);
            mockRepo.Setup(x => x.HasProductCategory(categoryId)).ReturnsAsync(false);

            mockRepo.Setup(x => x.IsCategoryExist(categoryId))
                .ReturnsAsync(true);

            mockRepo.Setup(x => x.RemoveAllFeaturesFromCategory(categoryId)).ThrowsAsync(new Exception());

            var result = await service.Remove(categoryId);


            Assert.False(result.Success);

            mockRepo.Verify(x => x.Remove(It.IsAny<int>()), Times.Never);
            mockRepo.Verify(x => x.RemoveAllFeaturesFromCategory(It.IsAny<int>()), Times.Once);

            Assert.Equal("Exception_Occured", result.ErrorCode);



        }
    }
}
