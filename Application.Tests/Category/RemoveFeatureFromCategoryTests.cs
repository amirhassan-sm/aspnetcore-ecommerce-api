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
    public class RemoveFeatureFromCategoryTests
    {
        private readonly Mock<ICategoryRepository> mockRepo;
        private readonly CategoryApplication service;
        public RemoveFeatureFromCategoryTests()
        {
            mockRepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mockRepo.Object);
        }
        [Fact]
        public async Task RemoveFeatureFromCategory_ShouldFail_WhenCategoryDoesNotExist()
        {
            var categorId= 10;
            var FeatureId = 13;


            mockRepo.Setup(x => x.IsCategoryExist(categorId)).ReturnsAsync(false);



            var result = await service.RemoveFeatureToCategory(categorId,FeatureId);

            Assert.False(result.Success);

            Assert.Equal("Category_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeatureToCategory(categorId, FeatureId), Times.Never);
            mockRepo.Verify(x => x.IsCategoryExist(categorId), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(FeatureId), Times.Never);

        }
        [Fact]
        public async Task RemoveFeatureFromCategory_ShouldFail_WhenFeatureDoesNotExist() {
            var categorId = 10;
            var FeatureId = 13;


            mockRepo.Setup(x => x.IsCategoryExist(categorId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(false);



            var result = await service.RemoveFeatureToCategory(categorId, FeatureId);

            Assert.False(result.Success);

            Assert.Equal("Feature_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeatureToCategory(categorId, FeatureId), Times.Never);
            mockRepo.Verify(x => x.IsCategoryExist(categorId), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(FeatureId), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryFeature(categorId, FeatureId), Times.Never);




        }
        [Fact]
        public async Task RemoveFeatureFromCategory_ShouldFail_WhenCategoryFeatureDoesNotExist()
        {
            var categorId = 10;
            var FeatureId = 13;


            mockRepo.Setup(x => x.IsCategoryExist(categorId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ExistCategoryFeature(categorId,FeatureId)).ReturnsAsync(false);



            var result = await service.RemoveFeatureToCategory(categorId, FeatureId);

            Assert.False(result.Success);

            Assert.Equal("CategoryFeature_DoesNot_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeatureToCategory(categorId, FeatureId), Times.Never);
            mockRepo.Verify(x => x.IsCategoryExist(categorId), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(FeatureId), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryFeature(categorId, FeatureId), Times.Once);




        }
        [Fact]
        public async Task RemoveFeatureFromCategory_ShouldSucceed()
        {
            var categorId = 10;
            var FeatureId = 13;
            var prodIds = new List<int> { 1, 2 };


            mockRepo.Setup(x => x.IsCategoryExist(categorId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ExistCategoryFeature(categorId, FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.RemoveFeatureToCategory(categorId, FeatureId)).ReturnsAsync(1);
            mockRepo.Setup(x => x.GetCategoryProductsId(categorId)).ReturnsAsync(prodIds);
            mockRepo.Setup(x => x.RemoveFeatureFromProductsAsync(FeatureId, prodIds)).Returns(Task.CompletedTask);






            var result = await service.RemoveFeatureToCategory(categorId, FeatureId);


            Assert.True(result.Success);

            mockRepo.Verify(x => x.RemoveFeatureToCategory(categorId, FeatureId), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(categorId), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(FeatureId), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryFeature(categorId, FeatureId), Times.Once);
            mockRepo.Verify(x => x.GetCategoryProductsId(categorId), Times.Once);
            mockRepo.Verify(x => x.RemoveFeatureFromProductsAsync(FeatureId, prodIds), Times.Once);






        }
        [Fact]
        public async Task RemoveFeatureFromCategory_ShouldFail_WhenExceptionThrows()
        {
            var categorId = 10;
            var FeatureId = 13;
            var prodIds = new List<int> { 1, 2 };


            mockRepo.Setup(x => x.IsCategoryExist(categorId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.IsFeatureExist(FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.ExistCategoryFeature(categorId, FeatureId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.RemoveFeatureToCategory(categorId, FeatureId)).Throws(new Exception { });
         






            var result = await service.RemoveFeatureToCategory(categorId, FeatureId);


            Assert.False(result.Success);
            Assert.Equal("Exception_Throws",result.ErrorCode);

            mockRepo.Verify(x => x.RemoveFeatureToCategory(categorId, FeatureId), Times.Once);
            mockRepo.Verify(x => x.IsCategoryExist(categorId), Times.Once);
            mockRepo.Verify(x => x.IsFeatureExist(FeatureId), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryFeature(categorId, FeatureId), Times.Once);
            mockRepo.Verify(x => x.GetCategoryProductsId(categorId), Times.Never);
            mockRepo.Verify(x => x.RemoveFeatureFromProductsAsync(FeatureId, prodIds), Times.Never);






        }
    }
}
