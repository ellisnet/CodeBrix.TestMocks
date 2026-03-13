using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class ThrowingRecursionBehaviorTest
{
    [Fact]
    public void SutIsSpecimenBuilderTransformation()
    {
        // Arrange
        // Act
        var sut = new ThrowingRecursionBehavior();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
    }

    [Fact]
    public void TransformNullBuilderThrows()
    {
        // Arrange
        var sut = new ThrowingRecursionBehavior();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Transform(null));
    }

    [Fact]
    public void TransformReturnsCorrectResult()
    {
        // Arrange
        var sut = new ThrowingRecursionBehavior();
        // Act
        var dummyBuilder = new DelegatingSpecimenBuilder();
        var result = sut.Transform(dummyBuilder);
        // Assert
        var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
        Assert.IsAssignableFrom<ThrowingRecursionHandler>(rg.RecursionHandler);
    }

    [Fact]
    public void TransformResultCorrectlyDecoratesInput()
    {
        // Arrange
        var sut = new ThrowingRecursionBehavior();
        var expectedBuilder = new DelegatingSpecimenBuilder();
        // Act
        var result = sut.Transform(expectedBuilder);
        // Assert
        var guard = Assert.IsAssignableFrom<RecursionGuard>(result);
        Assert.Equal(expectedBuilder, guard.Builder);
    }
}