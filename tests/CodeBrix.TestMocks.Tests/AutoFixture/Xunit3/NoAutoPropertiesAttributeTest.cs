using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation;
using System;
using System.Linq;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3; //was previously: namespace AutoFixture.Xunit3.UnitTest;

public class NoAutoPropertiesAttributeTest
{
    [Fact]
    public void SutIsAttribute()
    {
        // Arrange
        // Act
        var sut = new NoAutoPropertiesAttribute();
        // Assert
        Assert.IsAssignableFrom<CustomizeAttribute>(sut);
    }

    [Fact]
    public void GetCustomizationFromNullParameterThrows()
    {
        // Arrange
        var sut = new NoAutoPropertiesAttribute();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.GetCustomization(null));
    }

    [Fact]
    public void GetCustomizationReturnsTheCorrectResult()
    {
        // Arrange
        var sut = new NoAutoPropertiesAttribute();
        var parameter = TypeWithOverloadedMembers
            .GetDoSomethingMethod(typeof(object))
            .GetParameters().Single();
        // Act
        var result = sut.GetCustomization(parameter);
        // Assert
        Assert.IsAssignableFrom<NoAutoPropertiesCustomization>(result);
    }
}