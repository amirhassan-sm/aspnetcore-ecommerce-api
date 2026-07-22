using Application.DTOs.Category;
using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Category
{
    public class CategoryUpdateTest
    {
        private readonly Mock<ICategoryRepository> mockRepo;
        private readonly CategoryApplication Service;

        public CategoryUpdateTest()
        {
            mockRepo = new Mock<ICategoryRepository>();
            Service = new CategoryApplication(mockRepo.Object);
        }



        [Fact]

        public async Task Update_ShouldFail_WhenCategoryNotExist()
        {
            var cat = new CategoryUpdateModel
            {
                CategoryId = 1,
                CategoryName = "test",

            };
            mockRepo.Setup(x => x.Get(cat.CategoryId)).ReturnsAsync((Catalogue.Domain.Models.Category)null);

            var result = await Service.Update(cat);


            Assert.False(result.Success);
            Assert.Equal("Category_Not_Exist", result.ErrorCode);
            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);


        }
        [Fact]

        public async Task Update_ShouldFailed_WhenCategoryNameAlreadyExists()
        {
            var cat = new CategoryUpdateModel
            {
                CategoryId = 1,
                CategoryName = "test",

            };
            mockRepo.Setup(x => x.Get(cat.CategoryId)).ReturnsAsync(new Catalogue.Domain.Models.Category { CategoryId = 1, CategoryName = "test" });
            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(cat.CategoryId, cat.CategoryName)).ReturnsAsync(true);


            var result = await Service.Update(cat);

            Assert.False(result.Success);
            Assert.Equal("CategoryName_Already_exist", result.ErrorCode);
            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Get(cat.CategoryId), Times.Once);
            mockRepo.Verify(
      x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()),
      Times.Never);





        }

        [Fact]
        public async Task Update_Shouldfailed_WhenCategorySlugAlreadyExist()
        {
            var cat = new CategoryUpdateModel
            {
                CategoryId = 1,
                CategoryName = "test",
                Slug = "test",

            };
            mockRepo.Setup(x => x.Get(cat.CategoryId)).ReturnsAsync(new Catalogue.Domain.Models.Category { CategoryId = 1, CategoryName = "test" });
            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(cat.CategoryId, cat.CategoryName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(cat.CategoryId, cat.Slug)).ReturnsAsync(true);



            var result = await Service.Update(cat);

            Assert.False(result.Success);
            Assert.Equal("Category_Slug_AlreadyExist", result.ErrorCode);
            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Get(cat.CategoryId), Times.Once);
            mockRepo.Verify(
      x => x.GetChildNodeLevels(cat.CategoryId),
      Times.Never);



        }
        [Fact]
        public async Task Update_Shouldfailed_WhenCategoryIdAndParentAreSame()
        {

            var cat = new CategoryUpdateModel
            {
                CategoryId = 1,
                ParentId=1,
                CategoryName = "test",
                Slug = "test",

            };
            mockRepo.Setup(x => x.Get(cat.CategoryId)).ReturnsAsync(new Catalogue.Domain.Models.Category { CategoryId = 1, CategoryName = "test" });
            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(cat.CategoryId, cat.CategoryName)).ReturnsAsync(false);
            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(cat.CategoryId, cat.Slug)).ReturnsAsync(false);



            var result = await Service.Update(cat);

            Assert.False(result.Success);
            Assert.Equal("Category_CannotBe_OwnParent", result.ErrorCode);
            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Get(cat.CategoryId), Times.Once);
            mockRepo.Verify(
      x => x.GetChildNodeLevels(cat.CategoryId),
      Times.Never);







        }
        [Fact]
        public async Task Update_ShouldFail_WhenCategoryNewDepthExceedLimit()
        {
            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = null
            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = 1
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Get(1))
                    .ReturnsAsync(new Catalogue.Domain.Models.Category
                    {
                        CategoryId = 1,
                        Depth = 1,
                        LineAge = "1,"
                    });

            mockRepo.Setup(x => x.GetChildNodeLevels(model.CategoryId))
                    .ReturnsAsync(3);



            var result = await Service.Update(model);

            Assert.False(result.Success);
            Assert.Equal("Depth_Exceed_Limit", result.ErrorCode);
            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            //mockRepo.Verify(x => x.Get(It.IsAny<int>()), Times.AtMost(2));
            mockRepo.Verify(
      x => x.GetChildNodeLevels(model.CategoryId),
      Times.Once);
            mockRepo.Verify(
    x => x.GetChildren(model.CategoryId),
    Times.Never);







        }
        [Fact]
        public async Task Update_ShuoldFail_WhenNodeBecomeItsOwnChild()
        {

            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = null
            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = 1
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Get(1))
                    .ReturnsAsync(new Catalogue.Domain.Models.Category
                    {
                        CategoryId = 1,
                        Depth = 2,
                        LineAge = "1,2,"
                    });

            mockRepo.Setup(x => x.GetChildNodeLevels(model.CategoryId))
                    .ReturnsAsync(1);

            //mockRepo.Setup(x => x.GetChildren(model.CategoryId)).ReturnsAsync(new List<Catalogue.Domain.Models.Category>());



            var result = await Service.Update(model);


            Assert.False(result.Success);
            Assert.Equal("loop_tree", result.ErrorCode);
            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Never);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(
     x => x.GetChildNodeLevels(model.CategoryId),
     Times.Once);
            mockRepo.Verify(
    x => x.GetChildren(model.CategoryId),
    Times.Never);





        }

        [Fact]

        public async Task Update_ShouldUpdateOneChildren_WhenParentChanges()
        {

            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = null,

            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = 1
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Get(1))
                    .ReturnsAsync(new Catalogue.Domain.Models.Category
                    {
                        CategoryId = 1,
                        Depth = 1,
                        LineAge = "1,"
                    });

            mockRepo.Setup(x => x.GetChildNodeLevels(model.CategoryId))
                    .ReturnsAsync(2);

            mockRepo.Setup(x => x.GetChildren(model.CategoryId)).ReturnsAsync(new List<Catalogue.Domain.Models.Category> {
            new Catalogue.Domain.Models.Category{ CategoryId=3 ,
            ParentId= 2,
            LineAge="2,3,"
            },


            });
            mockRepo.Setup(x => x.Update(
                It.IsAny<Catalogue.Domain.Models.Category>()
            )).ReturnsAsync(true);




            var result = await Service.Update(model);

            Assert.True(result.Success);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(
     x => x.GetChildNodeLevels(model.CategoryId),
     Times.Once);
            mockRepo.Verify(
    x => x.GetChildren(model.CategoryId),
    Times.Once);

            mockRepo.Verify(x => x.SetDepthAndLineAge(3, 3, "1,2,3,"), Times.Once);

        }

        [Fact]

        public async Task Update_ShouldUpdateMultyChildren_WhenParentChanges()
        {
            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = null,

            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = 1
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Get(1))
                    .ReturnsAsync(new Catalogue.Domain.Models.Category
                    {
                        CategoryId = 1,
                        Depth = 1,
                        LineAge = "1,"
                    });

            mockRepo.Setup(x => x.GetChildNodeLevels(model.CategoryId))
                    .ReturnsAsync(2);

            mockRepo.Setup(x => x.GetChildren(model.CategoryId)).ReturnsAsync(new List<Catalogue.Domain.Models.Category> {
            new Catalogue.Domain.Models.Category{ CategoryId=3 ,
            ParentId= 2,
            LineAge="2,3,"
            },
             new Catalogue.Domain.Models.Category{ CategoryId=4 ,
            ParentId= 2,
            LineAge="2,4,"
            }


            });
            mockRepo.Setup(x => x.Update(
                It.IsAny<Catalogue.Domain.Models.Category>()
            )).ReturnsAsync(true);




            var result = await Service.Update(model);

            Assert.True(result.Success);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(
     x => x.GetChildNodeLevels(model.CategoryId),
     Times.Once);
            mockRepo.Verify(
    x => x.GetChildren(model.CategoryId),
    Times.Once);

            mockRepo.Verify(x => x.SetDepthAndLineAge(
                It.Is<int>(c => c == 4),
                It.Is<int>(c => c == 3),
                It.Is<string>(c => (c == "1,2,4,"))
), Times.Once);

            mockRepo.Verify(x => x.SetDepthAndLineAge(
             It.Is<int>(c => (c == 3)),
             It.Is<int>(c => c == 3),
             It.Is<string>(c => (c == "1,2,3,"))

             ), Times.Once);

        }

        [Fact]
        public async Task Update_ShouldUpdateChlidren_WhenOldParentLineageIsNotNull()
        {
            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = 1,


            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = 3
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Get(1))
                    .ReturnsAsync(new Catalogue.Domain.Models.Category
                    {
                        CategoryId = 1,
                        Depth = 1,
                        LineAge = "1,"
                    });
            mockRepo.Setup(x => x.Get(3))
                    .ReturnsAsync(new Catalogue.Domain.Models.Category
                    {
                        CategoryId = 3,
                        Depth = 1,
                        LineAge = "3,"
                    });

            mockRepo.Setup(x => x.GetChildNodeLevels(model.CategoryId))
                    .ReturnsAsync(2);

            mockRepo.Setup(x => x.GetChildren(model.CategoryId)).ReturnsAsync(new List<Catalogue.Domain.Models.Category> {
                new Catalogue.Domain.Models.Category{
                    CategoryId=2 ,
            ParentId= 1,
            LineAge="1,2,"
            },
            new Catalogue.Domain.Models.Category{ CategoryId=5 ,
            ParentId= 2,
            LineAge="1,2,5,"
            },
             new Catalogue.Domain.Models.Category{ CategoryId=6 ,
            ParentId= 2,
            LineAge="1,2,6,"
            }


            });
            mockRepo.Setup(x => x.Update(
                It.IsAny<Catalogue.Domain.Models.Category>()
            )).ReturnsAsync(true);




            var result = await Service.Update(model);

            Assert.True(result.Success);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(
     x => x.GetChildNodeLevels(model.CategoryId),
     Times.Once);
            mockRepo.Verify(
    x => x.GetChildren(model.CategoryId),
    Times.Once);

            mockRepo.Verify(x => x.SetDepthAndLineAge(
                It.Is<int>(c => c == 2),
                It.Is<int>(c => c == 2),
                It.Is<string>(c => (c == "3,2,"))
), Times.Once);

            mockRepo.Verify(x => x.SetDepthAndLineAge(
             It.Is<int>(c => (c == 5)),
             It.Is<int>(c => c == 3),
             It.Is<string>(c => (c == "3,2,5,"))

             ), Times.Once);

            mockRepo.Verify(x => x.SetDepthAndLineAge(
          It.Is<int>(c => (c == 6)),
          It.Is<int>(c => c == 3),
          It.Is<string>(c => (c == "3,2,6,"))

          ), Times.Once);
        }
        [Fact]

        public async Task Update_ShouldNotUpdateChlidren_WhenParntDoesNotChange()
        {
            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = null,

            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = null
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>())).ReturnsAsync(true);


            var result = await Service.Update(model);


            Assert.True(result.Success);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(
     x => x.GetChildNodeLevels(model.CategoryId),
     Times.Never);











        }
        [Fact]

        public async Task Update_ShouldReturnFailure_WhenRepositoryUpdateFails()
        {
            var oldNode = new Catalogue.Domain.Models.Category
            {
                CategoryId = 2,
                ParentId = null,

            };

            var model = new CategoryUpdateModel
            {
                CategoryId = 2,
                ParentId = null
            };

            mockRepo.Setup(x => x.Get(model.CategoryId))
                    .ReturnsAsync(oldNode);

            mockRepo.Setup(x => x.ExistCategoryNameExceptCurrentAsync(model.CategoryId, model.CategoryName))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.ExistCategorySlugExceptCurrentAsync(model.CategoryId, model.Slug))
                    .ReturnsAsync(false);

            mockRepo.Setup(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>())).ReturnsAsync(false);


            var result = await Service.Update(model);


            Assert.False(result.Success);
            Assert.Equal("Exception_Occured", result.ErrorCode);

            mockRepo.Verify(x => x.Update(It.IsAny<Catalogue.Domain.Models.Category>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategoryNameExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.ExistCategorySlugExceptCurrentAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            mockRepo.Verify(
     x => x.GetChildNodeLevels(model.CategoryId),
     Times.Never);





        }









    }
}
