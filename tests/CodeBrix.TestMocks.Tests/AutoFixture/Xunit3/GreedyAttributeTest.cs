using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation;
using System;
using System.Linq;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3; //was previously: namespace AutoFixture.Xunit3.UnitTest;

public class GreedyAttributeTest
{
    [Fact]
    public void SutIsAttribute()
    {
        // Arrange
        // Act
        var sut = new GreedyAttribute();
        // Assert
        Assert.IsAssignableFrom<CustomizeAttribute>(sut);
    }

    [Fact]
    public void GetCustomizationFromNullParameterThrows()
    {
        // Arrange
        var sut = new GreedyAttribute();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.GetCustomization(null));
    }

    [Fact]
    public void GetCustomizationReturnsCorrectResult()
    {
        // Arrange
        var sut = new GreedyAttribute();
        var parameter = typeof(TypeWithOverloadedMembers)
            .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), new[] { typeof(object) })!
            .GetParameters().Single();
        // Act
        var result = sut.GetCustomization(parameter);
        // Assert
        var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
        Assert.Equal(parameter.ParameterType, invoker.TargetType);
        Assert.IsAssignableFrom<GreedyConstructorQuery>(invoker.Query);
    }
}