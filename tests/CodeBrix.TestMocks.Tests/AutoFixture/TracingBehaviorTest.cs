using System;
using System.IO;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class TracingBehaviorTest
{
    [Fact]
    public void SutIsSpecimenBuilderTransformation()
    {
        // Arrange
        // Act
        var sut = new TracingBehavior();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
    }

    [Fact]
    public void InitializeWithNullWriterThrows()
    {
        // Arrange
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new TracingBehavior(null));
    }

    [Fact]
    public void WriterIsCorrectWhenExplicitlyProvided()
    {
        // Arrange
        var expectedWriter = new StringWriter();
        var sut = new TracingBehavior(expectedWriter);
        // Act
        TextWriter result = sut.Writer;
        // Assert
        Assert.Equal(expectedWriter, result);
    }

    [Fact]
    public void WriterIsCorrectWhenDefaultConstructorIsUsed()
    {
        // Arrange
        var sut = new TracingBehavior();
        // Act
        var result = sut.Writer;
        // Assert
        Assert.Equal(Console.Out, result);
    }

    [Fact]
    public void TransformNullBuilderThrows()
    {
        // Arrange
        var sut = new TracingBehavior();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Transform(null));
    }

    [Fact]
    public void TransformReturnsCorrectResult()
    {
        // Arrange
        var sut = new TracingBehavior();
        var builder = new DelegatingSpecimenBuilder();
        // Act
        var result = sut.Transform(builder);
        // Assert
        var tw = Assert.IsAssignableFrom<TraceWriter>(result);
        Assert.Equal(builder, tw.Tracer.Builder);
    }
}