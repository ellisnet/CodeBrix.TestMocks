using System.Linq;
using System.Reflection;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class AndPropertyQueryTest
{
    [Fact]
    public void SutIsPropertyQuery()
    {
        // Arrange
        // Act
        var sut = new AndPropertyQuery();

        // Assert
        Assert.IsAssignableFrom<IPropertyQuery>(sut);
    }

    [Fact]
    public void QueriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
    {
        // Arrange
        var sut = new AndPropertyQuery();

        // Act
        var result = sut.Queries;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void QueriesWillMatchProvidedParameters()
    {
        // Arrange
        var expectedResult = new IPropertyQuery[]
        {
            new DelegatingPropertyQuery(),
            new DelegatingPropertyQuery()
        };
        var sut = new AndPropertyQuery(expectedResult);

        // Act
        var result = sut.Queries;

        // Assert
        Assert.True(expectedResult.SequenceEqual(result));
    }

    [Fact]
    public void SelectPropertiesWillOnlySelectPropertiesMeetingProvidedQueryParameters()
    {
        // Arrange
        var stringProperty = typeof(PropertyHolder<string>).GetTypeInfo().GetProperties();
        var intProperty = typeof(PropertyHolder<int>).GetTypeInfo().GetProperties();

        var sut = new AndPropertyQuery(
            new DelegatingPropertyQuery
            {
                OnSelectProperties = _ => stringProperty.Concat(intProperty)
            },
            new DelegatingPropertyQuery
            {
                OnSelectProperties = _ => stringProperty
            });

        var expectedResult = stringProperty.AsEnumerable();

        // Act
        var result = sut.SelectProperties(typeof(string));

        // Assert
        Assert.True(expectedResult.SequenceEqual(result));
    }
}