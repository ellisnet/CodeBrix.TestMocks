using System;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

[Obsolete]
public class StableFiniteSequenceCustomizationTest
{
    [Fact]
    public void SutIsCustomization()
    {
        // Arrange
        // Act
        var sut = new StableFiniteSequenceCustomization();
        // Assert
        Assert.IsAssignableFrom<ICustomization>(sut);
    }

    [Fact]
    public void CustomizeNullFixtureThrows()
    {
        // Arrange
        var sut = new StableFiniteSequenceCustomization();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize(null));
    }

    [Fact]
    public void CustomizeAddsCorrectItemToCustomizations()
    {
        // Arrange
        var sut = new StableFiniteSequenceCustomization();
        var fixture = new Fixture();
        // Act
        sut.Customize(fixture);
        // Assert
        Assert.True(fixture.Customizations.OfType<StableFiniteSequenceRelay>().Any());
    }
}