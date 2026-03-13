using System;
using System.Globalization;
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Tests.AutoFixture.Kernel;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

[Obsolete]
public class InvariantCultureGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
#pragma warning disable 618
        var sut = new InvariantCultureGenerator();
#pragma warning restore 618
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnNoSpecimen()
    {
#pragma warning disable 618
        var sut = new InvariantCultureGenerator();
#pragma warning restore 618
        var actual = sut.Create(null, new DelegatingSpecimenContext());

        Assert.Equal(NoSpecimen.Instance, actual);
    }

    [Fact]
    public void CreateWithNullContextDoesNotThrow()
    {
#pragma warning disable 618
        var sut = new InvariantCultureGenerator();
#pragma warning restore 618
        sut.Create(new object(), null);
    }

    [Fact]
    public void CreateWithNonTypeRequestWillReturnNoSpecimen()
    {
#pragma warning disable 618
        var sut = new InvariantCultureGenerator();
#pragma warning restore 618
        var actual = sut.Create(new object(), new DelegatingSpecimenContext());

        Assert.Equal(NoSpecimen.Instance, actual);
    }

    [Fact]
    public void CreateWithNonCultureInfoTypeWillReturnNoSpecimen()
    {
#pragma warning disable 618
        var sut = new InvariantCultureGenerator();
#pragma warning restore 618
        var actual = sut.Create(typeof(object), new DelegatingSpecimenContext());

        Assert.Equal(NoSpecimen.Instance, actual);
    }

    [Fact]
    public void CreateWithCultureInfoRequestTypeReturnsInvariantCulture()
    {
#pragma warning disable 618
        var sut = new InvariantCultureGenerator();
#pragma warning restore 618
        var actual = sut.Create(typeof(CultureInfo), new DelegatingSpecimenContext());

        Assert.Equal(CultureInfo.InvariantCulture, actual);
    }
}