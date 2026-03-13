using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class DisposableTrackingCustomizationTest
{
    [Fact]
    public void SutIsCustomization()
    {
        // Arrange
        // Act
        var sut = new DisposableTrackingCustomization();
        // Assert
        Assert.IsAssignableFrom<ICustomization>(sut);
    }

    [Fact]
    public void CustomizeNullFixtureThrows()
    {
        // Arrange
        var sut = new DisposableTrackingCustomization();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize(null));
    }

    [Fact]
    public void BehaviorIsInstance()
    {
        // Arrange
        var sut = new DisposableTrackingCustomization();
        // Act
        DisposableTrackingBehavior result = sut.Behavior;
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void BehaviorIsStable()
    {
        // Arrange
        var sut = new DisposableTrackingCustomization();
        var expectedBehavior = sut.Behavior;
        // Act
        var result = sut.Behavior;
        // Assert
        Assert.Equal(expectedBehavior, result);
    }

    [Fact]
    public void CustomizeAddsCorrectBehavior()
    {
        // Arrange
        var fixture = new Fixture();
        var sut = new DisposableTrackingCustomization();
        // Act
        sut.Customize(fixture);
        // Assert
        Assert.Contains(sut.Behavior, fixture.Behaviors);
    }

    [Fact]
    public void SutIsDisposable()
    {
        // Arrange
        // Act
        var sut = new DisposableTrackingCustomization();
        // Assert
        Assert.IsAssignableFrom<IDisposable>(sut);
    }
}