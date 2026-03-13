using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class FalseRequestSpecificationTest
{
    [Fact]
    public void SutIsRequestSpecification()
    {
        // Arrange
        // Act
        var sut = new FalseRequestSpecification();
        // Assert
        Assert.IsAssignableFrom<IRequestSpecification>(sut);
    }

    [Fact]
    public void IsSatisfiedByReturnsCorrectResult()
    {
        // Arrange
        var sut = new FalseRequestSpecification();
        // Act
        var dummyRequest = new object();
        var result = sut.IsSatisfiedBy(dummyRequest);
        // Assert
        Assert.False(result);
    }
}