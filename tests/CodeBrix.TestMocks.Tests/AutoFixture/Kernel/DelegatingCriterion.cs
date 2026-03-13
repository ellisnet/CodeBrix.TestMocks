using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class DelegatingCriterion<T> : IEquatable<T>
{
    public DelegatingCriterion()
    {
        this.OnEquals = _ => false;
    }

    public bool Equals(T other)
    {
        return this.OnEquals(other);
    }

    public Func<T, bool> OnEquals { get; set; }
}