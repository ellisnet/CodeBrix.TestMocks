using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class TypeGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new TypeGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData("")]
    [InlineData(false)]
    [InlineData(true)]
    [InlineData(typeof(object))]
    [InlineData(typeof(string))]
    public void CreateNonTypeReturnsCorrectResult(object request)
    {
        // Arrange
        var sut = new TypeGenerator();
        // Act
        var dummyContext = new DelegatingSpecimenContext();
        var result = sut.Create(request, dummyContext);
        // Assert
        var expected = NoSpecimen.Instance;
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CreateTypeReturnsCorrectResult()
    {
        // Arrange
        var sut = new TypeGenerator();
        var request = typeof(Type);
        // Act
        var dummyContext = new DelegatingSpecimenContext();
        var result = sut.Create(request, dummyContext);
        // Assert
        var expected = typeof(object);
        Assert.Equal(expected, result);
    }
}