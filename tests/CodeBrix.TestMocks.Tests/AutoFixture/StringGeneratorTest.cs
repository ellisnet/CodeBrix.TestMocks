using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class StringGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new StringGenerator(() => new object());
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullFactoryWillThrow()
    {
        // Arrange
        Func<object> nullFactory = null;
        // Act
        // Assert (expected exception)
        Assert.Throws<ArgumentNullException>(() =>
            new StringGenerator(nullFactory));
    }

    [Fact]
    public void FactoryIsCorrect()
    {
        // Arrange
        Func<object> expectedFactory = () => new object();
        var sut = new StringGenerator(expectedFactory);
        // Act
        Func<object> result = sut.Factory;
        // Assert
        Assert.Equal(expectedFactory, result);
    }

    [Fact]
    public void CreateFromNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new StringGenerator(() => new object());
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(null, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithNullContainerWillNotThrow()
    {
        // Arrange
        var sut = new StringGenerator(() => string.Empty);
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateFromNonStringRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new StringGenerator(() => string.Empty);
        var nonStringRequest = new object();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonStringRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateFromStringRequestWillReturnCorrectResult()
    {
        // Arrange
        var specimen = 1;
        var expectedResult = specimen.ToString();

        var sut = new StringGenerator(() => specimen);
        var stringRequest = typeof(string);
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(stringRequest, dummyContainer);
        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateFromStringRequestWhenFactoryReturnsNullWillReturnCorrectResult()
    {
        // Arrange
        var sut = new StringGenerator(() => null);
        var stringRequest = typeof(string);
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(stringRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateFromStringRequestWhenFactoryReturnsNoSpecimenWillReturnCorrectResult()
    {
        // Arrange
        var expectedResult = NoSpecimen.Instance;
        var sut = new StringGenerator(() => expectedResult);
        var stringRequest = typeof(string);
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(stringRequest, dummyContainer);
        // Assert
        Assert.Equal(expectedResult, result);
    }
}