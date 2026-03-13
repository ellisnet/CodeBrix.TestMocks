using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

internal class DelegatingSpecimenContext : ISpecimenContext
{
    public DelegatingSpecimenContext()
    {
        this.OnResolve = r => null;
    }

    public object Resolve(object request)
    {
        return this.OnResolve(request);
    }

    internal Func<object, object> OnResolve { get; set; }
}