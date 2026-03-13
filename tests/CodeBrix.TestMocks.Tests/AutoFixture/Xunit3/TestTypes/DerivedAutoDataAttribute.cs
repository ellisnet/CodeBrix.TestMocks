using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class DerivedAutoDataAttribute : AutoDataAttribute
{
    public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
        : base(fixtureFactory)
    {
    }
}