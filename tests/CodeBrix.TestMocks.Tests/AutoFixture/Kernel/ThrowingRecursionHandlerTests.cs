using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class ThrowingRecursionHandlerTests
{
    [Fact]
    public void SutIsRecursionHandler()
    {
        // Arrange
        // Act
        var sut = new ThrowingRecursionHandler();
        // Assert
        Assert.IsAssignableFrom<IRecursionHandler>(sut);
    }

    [Fact]
    public void HandleRecursiveRequestThrows()
    {
        // Arrange
        var sut = new ThrowingRecursionHandler();
        // Act & assert
        Assert.ThrowsAny<ObjectCreationException>(
            () => sut.HandleRecursiveRequest(
                new object(),
                new[] { new object() }));
    }
}