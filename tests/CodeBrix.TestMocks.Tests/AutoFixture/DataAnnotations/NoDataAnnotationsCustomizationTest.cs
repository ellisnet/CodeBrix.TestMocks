using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.DataAnnotations;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.DataAnnotations;  //was previously: namespace AutoFixtureUnitTest.DataAnnotations;

public class NoDataAnnotationsCustomizationTest
{
    [Fact]
    public void SutIsCustomization()
    {
        // Act
        var sut = new NoDataAnnotationsCustomization();
        // Assert
        Assert.IsAssignableFrom<ICustomization>(sut);
    }

    [Fact]
    public void CustomizeNullFixtureThrows()
    {
        // Arrange
        var sut = new NoDataAnnotationsCustomization();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize(null));
    }

    [Fact]
    public void CustomizeProperFixtureCorrectlyCustomizesIt()
    {
        // Arrange
        var fixture = new Fixture();
        var sut = new NoDataAnnotationsCustomization();
        // Act
        sut.Customize(fixture);
        // Assert
        Assert.DoesNotContain(
            fixture.Customizations,
            b => b is DataAnnotationsSupportNode);
    }
}