using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using System;
using System.Reflection;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

internal class DelegatingCustomizeAttribute : CustomizeAttribute
{
    public DelegatingCustomizeAttribute()
    {
        this.OnGetCustomization = p => new DelegatingCustomization();
    }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        return this.OnGetCustomization(parameter);
    }

    public Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
}