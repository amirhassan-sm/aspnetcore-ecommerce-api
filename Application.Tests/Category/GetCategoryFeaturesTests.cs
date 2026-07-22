using Application.DTOs.CategoryFeatureModel;
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
    public class GetCategoryFeaturesTests
    {
        private readonly Mock<ICategoryRepository> mockRepo;
        private readonly CategoryApplication service;
        public GetCategoryFeaturesTests()
        {
            mockRepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mockRepo.Object);
        }

        [Fact]
        public async Task GetCategoryFeatures_ShouldFail_WhenCategoryNotExist()
        {
            var categoryId = 11;
            mockRepo.Setup(x=>x.IsCategoryExist(categoryId)).ReturnsAsync(false);


            var result = await service.GetCategoryFeatures(categoryId);


            Assert.False(result.Success);
            Assert.Equal("Category_Not_Exist", result.ErrorCode);

            mockRepo.Verify(x => x.IsCategoryExist(categoryId), Times.Once);
            mockRepo.Verify(x => x.GetCategoryFeatures(categoryId), Times.Never);



        }
        [Fact]
        public async Task GetCategoryFeatures_ShouldSucceed()
        {
            var categoryId = 11;
            var CategoryFeatures = new List<Catalogue.Domain.Models.CategoryFeature> {
            new Catalogue.Domain.Models.CategoryFeature{
                CategoryFeatureId = 1,
            CategoryId = 11,
            FeatureId = 2,

            
            },



            };
            var model = new CategoryFeatureModel {
                CategoryFeatureId = 1,
                CategoryId = 11,
                FeatureId = 2,


            };
            mockRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.GetCategoryFeatures(categoryId)).ReturnsAsync(CategoryFeatures);


            var result = await service.GetCategoryFeatures(categoryId);


            Assert.True(result.Success);

            mockRepo.Verify(x => x.IsCategoryExist(categoryId), Times.Once);
            mockRepo.Verify(x => x.GetCategoryFeatures(categoryId), Times.Once);
            Assert.Single(result.Item);




        }

        [Fact]
        public async Task GetCategoryFeatures_ShouldFail_WhenExceptionThrows()
        {
            var categoryId = 11;
            var CategoryFeatures = new List<Catalogue.Domain.Models.CategoryFeature> {
            new Catalogue.Domain.Models.CategoryFeature{
                CategoryFeatureId = 1,
            CategoryId = 11,
            FeatureId = 2,


            },



            };
            var model = new CategoryFeatureModel
            {
                CategoryFeatureId = 1,
                CategoryId = 11,
                FeatureId = 2,


            };
            mockRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mockRepo.Setup(x => x.GetCategoryFeatures(categoryId)).Throws(new Exception { });


            var result = await service.GetCategoryFeatures(categoryId);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);

            mockRepo.Verify(x => x.IsCategoryExist(categoryId), Times.Once);
            mockRepo.Verify(x => x.GetCategoryFeatures(categoryId), Times.Once);




        }
    }
}
