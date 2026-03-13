using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class CreatingAbstractClassWithPublicConstructorTests
{
    [Fact]
    public void CreateAbstractWithPublicConstructorWillThrow()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create<AbstractClassWithPublicConstructor>());
    }

    [Fact]
    public void MapAbstractClassWithPublicConstructorToTestDoubleToWorkAroundException()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(AbstractClassWithPublicConstructor),
                typeof(TestDouble)));
        // Act
        var actual = fixture.Create<AbstractClassWithPublicConstructor>();
        // Assert
        Assert.IsAssignableFrom<TestDouble>(actual);
    }

    private class TestDouble : AbstractClassWithPublicConstructor
    {
    }
}