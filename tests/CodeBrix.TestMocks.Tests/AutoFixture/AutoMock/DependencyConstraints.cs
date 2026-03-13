using CodeBrix.TestMocks.AutoFixture.AutoMock;
using System.Reflection;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock; //was previously: namespace AutoFixture.AutoMoq.UnitTest;

public class DependencyConstraints
{
    [Theory]
    [InlineData("FakeItEasy")]
    [InlineData("Rhino.Mocks")]
    [InlineData("xunit")]
    [InlineData("xunit.extensions")]
    public void AutoMockDoesNotReference(string assemblyName)
    {
        // Arrange
        // Act
        var references = typeof(AutoMockCustomization).GetTypeInfo().Assembly.GetReferencedAssemblies();
        // Assert
        Assert.DoesNotContain(references, an => an.Name == assemblyName);
    }

    [Theory]
    [InlineData("FakeItEasy")]
    [InlineData("Rhino.Mocks")]
    public void AutoFixtureUnitTestsDoNotReference(string assemblyName)
    {
        // Arrange
        // Act
        var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
        // Assert
        Assert.DoesNotContain(references, an => an.Name == assemblyName);
    }
}