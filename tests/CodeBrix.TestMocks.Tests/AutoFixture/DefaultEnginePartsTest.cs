using System;
using System.Collections.Generic;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class DefaultEnginePartsTest
{
    [Fact]
    public void SutIsTransmissionBuilders()
    {
        // Arrange
        // Act
        var sut = new DefaultEngineParts();
        // Assert
        Assert.IsAssignableFrom<DefaultRelays>(sut);
    }

    [Fact]
    public void InitializedWithDefaultConstructorSutHasCorrectContents()
    {
        // Arrange
        var expectedBuilders = new DefaultPrimitiveBuilders()
            .Concat(new DefaultRelays())
            .Select(b => b.GetType());
        // Act
        var sut = new DefaultEngineParts();
        // Assert
        Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
    }

    [Fact]
    public void InitializeWithNullBuildersThrows()
    {
        // Arrange
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new DefaultEngineParts((IEnumerable<ISpecimenBuilder>)null));
    }

    [Fact]
    public void InitializedWithEnumerableBuildersSutHasCorrectContents()
    {
        // Arrange
        var primitiveBuilders = Enumerable.Range(1, 3).Select(i => new DelegatingSpecimenBuilder()).Cast<ISpecimenBuilder>().ToList();
        var expectedBuilders = primitiveBuilders
            .Concat(new DefaultRelays())
            .Select(b => b.GetType());
        // Act
        var sut = new DefaultEngineParts(primitiveBuilders);
        // Assert
        Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
    }

    [Fact]
    public void InitializeWithNullArrayThrows()
    {
        // Arrange
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new DefaultEngineParts((ISpecimenBuilder[])null));
    }

    [Fact]
    public void InitializedWithBuildersArraySutHasCorrectContents()
    {
        // Arrange
        var primitiveBuilders = Enumerable.Range(1, 3).Select(i => new DelegatingSpecimenBuilder()).ToArray();
        var expectedBuilders = primitiveBuilders
            .Concat(new DefaultRelays())
            .Select(b => b.GetType());
        // Act
        var sut = new DefaultEngineParts(primitiveBuilders);
        // Assert
        Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
    }
}