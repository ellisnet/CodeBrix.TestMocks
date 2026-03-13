using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

internal class DerivedInlineAutoDataAttribute : InlineAutoDataAttribute
{
    public DerivedInlineAutoDataAttribute(Func<IFixture> fixtureFactory, params object[] values)
        : base(fixtureFactory, values)
    {
    }
}