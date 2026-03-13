using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class CustomizationExtensionsTest
{
    [Fact]
    public void ToCustomization_ShouldThrowIfBuilderIsNull()
    {
        // Arrange
        ISpecimenBuilder nullBuilder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            nullBuilder.ToCustomization());
    }

    [Fact]
    public void ToCustomization_ShouldThrowIfNullFixturePassedToCustomization()
    {
        // Arrange
        var sut = new DelegatingSpecimenBuilder().ToCustomization();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize(fixture: null));
    }

    [Fact]
    public void ToCustomization_ReturnedCustomizationShouldInsertAtTheBeginning()
    {
        // Arrange
        var builder = new DelegatingSpecimenBuilder();
        var fixture = new Fixture();

        // Act
        var sut = builder.ToCustomization();
        fixture.Customize(sut);

        // Assert
        Assert.Same(builder, fixture.Customizations[0]);
    }
}