using Application.DTOs.Category;
using Application.implementation;
using Catalogue.Domain.Models;
using Catalogue.DomainServiceContract.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Category
{
    public class CategoryAddTests
    {
        private readonly Mock<ICategoryRepository> mockrepo;
        private readonly CategoryApplication service;
        public CategoryAddTests()
        {
            mockrepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mockrepo.Object);
        }
        [Fact]
        public async Task AddNew_ShouldFail_WhenCategoryNameAlreadyExists()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",



            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(true);

            var result = await service.AddNew(category);

            Assert.False(result.Success);
            Assert.Equal("CategoryName_Already_Exist", result.ErrorCode);

            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockrepo.Verify(x => x.ExistSlugName(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public async Task AddNew_ShouldFail_WhenSlugNameAlreadyExists()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",
                Slug = "test"
            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);


            mockrepo.Setup(x => x.ExistSlugName(category
                .Slug)).ReturnsAsync(true);


            var result = await service.AddNew(category);

            Assert.False(result.Success);
            Assert.Equal("Slug_Already_Exist", result.ErrorCode);
            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockrepo.Verify(x => x.Get(It.IsAny<int>()), Times.Never);

        }

        [Fact]

        public async Task AddNew_ShouldFailed_WhenParentDoesNotExist()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",
                Slug = "test",
                ParentId = 1
            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);


            mockrepo.Setup(x => x.ExistSlugName(category
                .Slug)).ReturnsAsync(false);

            mockrepo.Setup(x => x.Get(category.ParentId.Value)).ReturnsAsync((Catalogue.Domain.Models.Category)null);

            var result = await service.AddNew(category);

            Assert.False(result.Success);
            Assert.Equal("Parent_Is_Null", result.ErrorCode);
            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);

        }
        [Fact]

        public async Task AddNew_ShouldFail_WhenParentDepthIsThree()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",
                Slug = "test",
                ParentId = 1
            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);


            mockrepo.Setup(x => x.ExistSlugName(category
                .Slug)).ReturnsAsync(false);

            mockrepo.Setup(x => x.Get(category.ParentId.Value)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryName = "Test",
                CategoryId = 1,

                Depth = 3
            });


            var result = await service.AddNew(category);

            Assert.False(result.Success);
            Assert.Equal("Depth_Is_BiggerThan3", result.ErrorCode);

            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);




        }

        [Fact]
        public async Task AddNew_Child_ShouldSucceed_WhenParentDepthIsNull_AndRepairIt()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Level3",
                CategoryDescription = "Test",
                Slug = "level-3",
                ParentId = 2
            };

            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);
            mockrepo.Setup(x => x.ExistSlugName(category.Slug)).ReturnsAsync(false);
            mockrepo.Setup(x => x.Get(2)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                CategoryName = "Level2",
                ParentId = 1,
                LineAge = "1,2,",
                Depth = null // broken row that previously crashed on .Value
            });
            mockrepo.Setup(x => x.Get(1)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryId = 1,
                CategoryName = "Level1",
                ParentId = null,
                LineAge = "1,",
                Depth = 1
            });
            mockrepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>())).ReturnsAsync(3);
            mockrepo.Setup(x => x.SetDepthAndLineAge(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await service.AddNew(category);

            Assert.True(result.Success);
            mockrepo.Verify(x => x.SetDepthAndLineAge(2, 2, "1,2,"), Times.Once);
            mockrepo.Verify(x => x.SetDepthAndLineAge(3, 3, "1,2,3,"), Times.Once);
        }

        [Fact]
        public async Task AddNew_Child_ShouldSuccess()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",
                Slug = "test",
                ParentId = 1
            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);


            mockrepo.Setup(x => x.ExistSlugName(category
                .Slug)).ReturnsAsync(false);

            mockrepo.Setup(x => x.Get(category.ParentId.Value)).ReturnsAsync(new Catalogue.Domain.Models.Category
            {
                CategoryName = "Test",
                CategoryId = 1,
                LineAge="1,",

                Depth = 1
            });

            mockrepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>())).ReturnsAsync(2);
            mockrepo.Setup(x => x.SetDepthAndLineAge(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
                )).ReturnsAsync(true);

            var result = await service.AddNew(category);

            Assert.True(result.Success);
            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockrepo.Verify(
       x => x.SetDepthAndLineAge(
           2,
           2,
           "1,2,"),
       Times.Once());


        }

        [Fact]
        public async Task AddNew_Root_ShouldSuccess()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",
                Slug = "test",
                ParentId = null
            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);


            mockrepo.Setup(x => x.ExistSlugName(category
                .Slug)).ReturnsAsync(false);



            mockrepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>())).ReturnsAsync(1);
            mockrepo.Setup(x => x.SetDepthAndLineAge(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
                )).ReturnsAsync(true);

            var result = await service.AddNew(category);

            Assert.True(result.Success);
            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockrepo.Verify(x => x.Get(It.IsAny<int>()), Times.Never);
            mockrepo.Verify(
       x => x.SetDepthAndLineAge(
           1,
           1,
           "1,"),
       Times.Once());

        }

        [Fact]
        public async Task AddNew_ShuoldFailed_WhenExceptionThrows()
        {
            var category = new CategoryAddModel
            {
                CategoryName = "Test",
                CategoryDescription = "Test",
                Slug = "test",
                ParentId = null
            };
            mockrepo.Setup(x => x.ExistCategoryName(category.CategoryName)).ReturnsAsync(false);


            mockrepo.Setup(x => x.ExistSlugName(category
                .Slug)).ReturnsAsync(false);



            mockrepo.Setup(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>())).ThrowsAsync(new Exception());
         

            var result = await service.AddNew(category);

            Assert.False(result.Success);
            Assert.Equal("ExcetionThrows",result.ErrorCode);
            mockrepo.Verify(x => x.AddNew(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockrepo.Verify(x => x.Get(It.IsAny<int>()), Times.Never);
            mockrepo.Verify(
       x => x.SetDepthAndLineAge(
                          It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()),
       Times.Never());



        }

    }
}
