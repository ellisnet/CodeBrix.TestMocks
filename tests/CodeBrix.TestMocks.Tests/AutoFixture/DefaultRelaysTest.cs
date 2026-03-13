using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.DataAnnotations;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class DefaultRelaysTest
{
    [Fact]
    public void SutIsSpecimenBuilders()
    {
        // Arrange
        // Act
        var sut = new DefaultRelays();
        // Assert
        Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
    }

    [Fact]
    public void SutHasCorrectContents()
    {
        // Arrange
        var expectedBuilderTypes = new[]
        {
            typeof(LazyRelay),
            typeof(MultidimensionalArrayRelay),
            typeof(ArrayRelay),
            typeof(ParameterRequestRelay),
            typeof(PropertyRequestRelay),
            typeof(FieldRequestRelay),
            typeof(RangedSequenceRelay),
            typeof(FiniteSequenceRelay),
            typeof(SeedIgnoringRelay),
            typeof(MethodInvoker)
        };
        // Act
        var sut = new DefaultRelays();
        // Assert
        Assert.True(expectedBuilderTypes.SequenceEqual(sut.Select(b => b.GetType())));
    }

    [Fact]
    public void NonGenericEnumeratorMatchesGenericEnumerator()
    {
        // Arrange
        var sut = new DefaultRelays();
        // Act
        IEnumerable result = sut;
        // Assert
        Assert.True(sut.Select(b => b.GetType()).SequenceEqual(result.Cast<object>().Select(o => o.GetType())));
    }
}