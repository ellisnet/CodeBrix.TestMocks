using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public abstract class FixtureResolvingItselfTests<T>
{
    [Fact]
    public void FixtureCanResolveItself()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var actual = sut.Create<T>();
        // Assert
        Assert.Equal<object>(sut, actual);
    }
}

public class FixtureResolvingItselfTestsOfFixture : FixtureResolvingItselfTests<Fixture>
{
}
public class FixtureResolvingItselfTestsOfFixtureInterface : FixtureResolvingItselfTests<IFixture>
{
}
public class FixtureResolvingItselfTestsOfSpecimenBuilder : FixtureResolvingItselfTests<ISpecimenBuilder>
{
}