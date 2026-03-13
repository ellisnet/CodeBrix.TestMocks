using CodeBrix.TestMocks.AutoFixture;
using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

internal class DelegatingCustomization : ICustomization
{
    internal DelegatingCustomization()
    {
        this.OnCustomize = _ => { };
    }

    public void Customize(IFixture fixture)
    {
        this.OnCustomize(fixture);
    }

    internal Action<IFixture> OnCustomize { get; set; }
}