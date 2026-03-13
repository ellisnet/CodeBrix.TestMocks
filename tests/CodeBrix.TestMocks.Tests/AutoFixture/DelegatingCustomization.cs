using System;
using CodeBrix.TestMocks.AutoFixture;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

internal class DelegatingCustomization : ICustomization
{
    public DelegatingCustomization()
    {
        this.OnCustomize = f => { };
    }

    public void Customize(IFixture fixture)
    {
        this.OnCustomize(fixture);
    }

    internal Action<IFixture> OnCustomize { get; set; }
}