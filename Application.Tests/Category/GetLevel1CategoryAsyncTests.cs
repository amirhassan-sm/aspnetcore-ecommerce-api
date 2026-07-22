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
    public class GetLevel1CategoryAsyncTests
    {
        private readonly Mock<ICategoryRepository> mockrepo;
        private readonly CategoryApplication service;
        public GetLevel1CategoryAsyncTests()
        {
            mockrepo = new Mock<ICategoryRepository>();
            service = new CategoryApplication(mockrepo.Object);
        }
        public async Task GetLevel1Category_ShouldSucceed()
        {
            var categories = new List<Catalogue.Domain.Models.Category>
{
    new Catalogue.Domain.Models.Category
    {
        CategoryId = 1,
        CategoryName = "Electronics",
        CategoryDescription = "Devices",
        Depth = 1,
        LineAge = "1,",
        ParentId = null,
        Slug = "electronics"
    }
};

            mockrepo.Setup(x => x.GetLevel1CategoryAsync()).ReturnsAsync(categories);


            var result = await service.GetLevel1CategoryAsync();




            Assert.True(result.Success);
            Assert.Single(result.Item);

            var item = result.Item.Single();

            Assert.Equal(1, item.CategoryId);
            Assert.Equal("Electronics", item.CategoryName);
            Assert.Equal("Devices", item.CategoryDescription);
            Assert.Equal(1, item.Depth);
            Assert.Equal("1,", item.LineAge);
            Assert.Null(item.ParentId);
            Assert.Equal("electronics", item.Slug);

            mockrepo.Verify(x => x.GetLevel1CategoryAsync(), Times.Once);





        }



        public async Task GetLevel1Category_ShouldFail_WhenExceptionThrows()
        {

            mockrepo.Setup(x => x.GetLevel1CategoryAsync()).Throws(new Exception());


            var result = await service.GetLevel1CategoryAsync();


            Assert.False(result.Success);


            Assert.Equal("Exception_Occured",result.ErrorCode);
        }
    }
}
