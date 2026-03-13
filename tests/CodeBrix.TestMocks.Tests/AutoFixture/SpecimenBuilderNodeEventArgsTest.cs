using System;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class SpecimenBuilderNodeEventArgsTest
{
    [Fact]
    public void SutIsEventArgs()
    {
        // Arrange
        var dummyNode = new CompositeSpecimenBuilder();
        // Act
        var sut = new SpecimenBuilderNodeEventArgs(dummyNode);
        // Assert
        Assert.IsAssignableFrom<EventArgs>(sut);
    }

    [Fact]
    public void GraphIsCorrect()
    {
        // Arrange
        var expected = new CompositeSpecimenBuilder();
        var sut = new SpecimenBuilderNodeEventArgs(expected);
        // Act
        ISpecimenBuilderNode actual = sut.Graph;
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConstructWithNullGraphThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new SpecimenBuilderNodeEventArgs(null));
    }
}