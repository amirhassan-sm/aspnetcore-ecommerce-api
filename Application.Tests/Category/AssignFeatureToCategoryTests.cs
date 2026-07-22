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
    public class AssignFeatureToCategoryTests
    {
        private readonly Mock<ICategoryRepository> mocklRepo;
        private readonly CategoryApplication service;
        public AssignFeatureToCategoryTests()
        {
            mocklRepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mocklRepo.Object);
        }

        [Fact]
        public async Task Assign_ShouldFail_WhenCategoryNotExist()
        {
            var categoryId = 1;
            var featureId = 1;

            mocklRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(false);


            var result = await service.AssignFeatureToCategory(categoryId, featureId);


            Assert.False(result.Success);

            Assert.Equal("Categor_Not_exist", result.ErrorCode);



            mocklRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);
            mocklRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Never);
            mocklRepo.Verify(x => x.AssignFeatureToCategory(It.IsAny<int>(), It.IsAny<int>()), Times.Never);





        }
        [Fact]
        public async Task Assign_ShouldFail_WhenFeatureNotExist()
        {
            var categoryId = 1;
            var featureId = 1;

            mocklRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.IsFeatureExist(featureId)).ReturnsAsync(false);



            var result = await service.AssignFeatureToCategory(categoryId, featureId);


            Assert.False(result.Success);


            Assert.Equal("Feature_Not_Exist", result.ErrorCode);



            mocklRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.ExistCategoryFeature(It.IsAny<int>(), It.IsAny<int>()), Times.Never);

            mocklRepo.Verify(x => x.AssignFeatureToCategory(It.IsAny<int>(), It.IsAny<int>()), Times.Never);



        }
        [Fact]
        public async Task Assign_ShouldFail_WhenCategoryFeatureAlreadyExist()
        {
            var categoryId = 1;
            var featureId = 1;

            mocklRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.IsFeatureExist(featureId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.ExistCategoryFeature(categoryId, featureId)).ReturnsAsync(true);




            var result = await service.AssignFeatureToCategory(categoryId, featureId);


            Assert.False(result.Success);


            Assert.Equal("CategoryFeature_Already_Exist", result.ErrorCode);



            mocklRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.ExistCategoryFeature(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.Get(It.IsAny<int>()), Times.Never);

            mocklRepo.Verify(x => x.AssignFeatureToCategory(It.IsAny<int>(), It.IsAny<int>()), Times.Never);



        }
        [Fact]

        public async Task Assign_ShouldFail_WhenCategoryDepthIsMoreThan3()
        {
            var categoryId = 1;
            var featureId = 1;



            mocklRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.IsFeatureExist(featureId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.ExistCategoryFeature(categoryId, featureId)).ReturnsAsync(false);
            mocklRepo.Setup(x => x.Get(categoryId)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryId = 1,
                Depth = 1,
            });






            var result = await service.AssignFeatureToCategory(categoryId, featureId);


            Assert.False(result.Success);


            Assert.Equal("Category_IsNot_Level3", result.ErrorCode);



            mocklRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.ExistCategoryFeature(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.Get(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.AssignFeatureToCategory(It.IsAny<int>(), It.IsAny<int>()), Times.Never);

            





        }

        [Fact]
        public async Task Assign_ShouldSucceed()
        {
            var categoryId = 1;
            var featureId = 1;



            mocklRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.IsFeatureExist(featureId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.ExistCategoryFeature(categoryId, featureId)).ReturnsAsync(false);
            mocklRepo.Setup(x => x.Get(categoryId)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryId = 1,
                Depth = 3,
            });
            mocklRepo.Setup(x => x.AssignFeatureToCategory(categoryId, featureId))
         .ReturnsAsync(10);

            mocklRepo.Setup(x => x.GetCategoryProductsId(categoryId))
                     .ReturnsAsync(new List<int> { 100, 101 });

            mocklRepo.Setup(x => x.AssignFeatureToProductsAsync(
                    featureId,
                    It.IsAny<List<int>>()))
                .Returns(Task.CompletedTask);






            var result = await service.AssignFeatureToCategory(categoryId, featureId);


            Assert.True(result.Success);





            mocklRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.ExistCategoryFeature(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.Get(It.IsAny<int>()), Times.Once);


            mocklRepo.Verify(x => x.AssignFeatureToCategory(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mocklRepo.Verify(x => x.GetCategoryProductsId(It.IsAny<int>()), Times.Once);
            mocklRepo.Verify(x => x.AssignFeatureToProductsAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Once);




        }
        [Fact]
        public async Task Assign_ShouldFail_WhenExceptionThrows() {
            var categoryId = 1;
            var featureId = 1;



            mocklRepo.Setup(x => x.IsCategoryExist(categoryId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.IsFeatureExist(featureId)).ReturnsAsync(true);
            mocklRepo.Setup(x => x.ExistCategoryFeature(categoryId, featureId)).ReturnsAsync(false);
            mocklRepo.Setup(x => x.Get(categoryId)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryId = 1,
                Depth = 3,
            });
            mocklRepo.Setup(x => x.AssignFeatureToCategory(categoryId, featureId))
         .ReturnsAsync(10);

            mocklRepo.Setup(x => x.GetCategoryProductsId(categoryId))
                     .ReturnsAsync(new List<int> { 100, 101 });

            mocklRepo.Setup(x => x.AssignFeatureToProductsAsync(
                    featureId,
                    It.IsAny<List<int>>()))
                .Throws( new Exception { } );







            var result = await service.AssignFeatureToCategory(categoryId, featureId);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured",result.ErrorCode);





            mocklRepo.Verify(x => x.IsCategoryExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.IsFeatureExist(It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.ExistCategoryFeature(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            mocklRepo.Verify(x => x.Get(It.IsAny<int>()), Times.Once);


            mocklRepo.Verify(x => x.AssignFeatureToCategory(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mocklRepo.Verify(x => x.GetCategoryProductsId(It.IsAny<int>()), Times.Once);
            mocklRepo.Verify(x => x.AssignFeatureToProductsAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Once);






        }


    }
}
