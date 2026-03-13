using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class IllegalRequestExceptionTest
{
    [Fact]
    public void SutIsException()
    {
        // Arrange
        // Act
        var sut = new IllegalRequestException();
        // Assert
        Assert.IsAssignableFrom<Exception>(sut);
    }

    [Fact]
    public void MessageWillBeDefineWhenDefaultConstructorIsUsed()
    {
        // Arrange
        var sut = new IllegalRequestException();
        // Act
        var result = sut.Message;
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void MessageWillMatchConstructorArgument()
    {
        // Arrange
        string expectedMessage = "Anonymous exception message";
        var sut = new IllegalRequestException(expectedMessage);
        // Act
        var result = sut.Message;
        // Assert
        Assert.Equal(expectedMessage, result);
    }

    [Fact]
    public void InnerExceptionWillMatchConstructorArgument()
    {
        // Arrange
        var expectedException = new ArgumentOutOfRangeException();
        var sut = new IllegalRequestException("Anonymous message.", expectedException);
        // Act
        var result = sut.InnerException;
        // Assert
        Assert.Equal(expectedException, result);
    }
}