using System;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

[Obsolete]
public class MapCreateManyToEnumerableTests
{
    [Fact]
    public void SutIsCustomization()
    {
        var sut = new MapCreateManyToEnumerable();
        Assert.IsAssignableFrom<ICustomization>(sut);
    }

    [Fact]
    public void CustomizeAddsCorrectSpecimenBuilderToFixture()
    {
        // Arrange
        var sut = new MapCreateManyToEnumerable();
        var fixture = new Fixture();
        // Act
        sut.Customize(fixture);
        // Assert
        Assert.True(
            fixture.Customizations.OfType<MultipleToEnumerableRelay>().Any(),
            "Appropriate SpecimenBuilder should be added to Fixture.");
    }

    [Fact]
    public void CustomizeNullFixtureThrows()
    {
        // Arrange
        var sut = new MapCreateManyToEnumerable();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
    }
}