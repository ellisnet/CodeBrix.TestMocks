using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

internal class DelegatingRequestSpecification : IRequestSpecification
{
    public DelegatingRequestSpecification()
    {
        this.OnIsSatisfiedBy = r => false;
    }

    public bool IsSatisfiedBy(object request)
    {
        return this.OnIsSatisfiedBy(request);
    }

    internal Predicate<object> OnIsSatisfiedBy { get; set; }
}