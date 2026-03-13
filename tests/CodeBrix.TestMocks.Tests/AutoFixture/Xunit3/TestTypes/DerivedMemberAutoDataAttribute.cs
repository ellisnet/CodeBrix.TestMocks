using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class DerivedMemberAutoDataAttribute : MemberAutoDataAttribute
{
    public DerivedMemberAutoDataAttribute(Func<IFixture> fixtureFactory, string memberName, params object[] parameters)
        : base(fixtureFactory, memberName, parameters)
    {
    }

    public DerivedMemberAutoDataAttribute(Func<IFixture> fixtureFactory, Type memberType, string memberName, params object[] parameters)
        : base(fixtureFactory, memberType, memberName, parameters)
    {
    }
}