namespace Application.Tests;

using Application.Contrast;
using Application.Contrast.Services;
using Catalogue.DomainServiceContract.Service;
using Moq;
using Xunit;
// you can use fluent assertion
public class UnitTest1
{
    private readonly Mock<ICategoryRepository> categoryrepo;
    private readonly Mock<ICategoryApplicationServiceContract> catApp;
    public UnitTest1()
    {
        categoryrepo = new Mock<ICategoryRepository>();
        catApp = new Mock<ICategoryApplicationServiceContract>();

    }
    public int add()
    {
        return 1;
    }
    public bool IsOdd(int value)
    {
        return (value % 2 != 0);
    }
    //[Fact]
    //[Trait("dick", "dicker")]
    //public void Test1()
    //{
    //    var sum = add();

    //    Assert.True(true);

    //}

    [Theory]
    [InlineData(2, false)]
    //[MemberData()]
    public void IsOddTest(int value , bool expected)
    {
        var x = new UnitTest1();
        bool result = x.IsOdd(value);
        Assert.Equal(expected,result);
    }
}
