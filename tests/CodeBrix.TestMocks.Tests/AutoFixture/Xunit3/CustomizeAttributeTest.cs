using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes;
using System;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3; //was previously: namespace AutoFixture.Xunit3.UnitTest;

public class CustomizeAttributeTest
{
    [Fact]
    public void TestableSutIsSut()
    {
        // Arrange
        // Act
        var sut = new DelegatingCustomizeAttribute();
        // Assert
        Assert.IsAssignableFrom<CustomizeAttribute>(sut);
    }

    [Fact]
    public void SutIsAttribute()
    {
        // Arrange
        // Act
        var sut = new DelegatingCustomizeAttribute();
        // Assert
        Assert.IsAssignableFrom<Attribute>(sut);
    }

    [Fact]
    public void SutImplementsIParameterCustomizationSource()
    {
        // Arrange
        // Act
        var sut = new DelegatingCustomizeAttribute();
        // Assert
        Assert.IsAssignableFrom<IParameterCustomizationSource>(sut);
    }
}