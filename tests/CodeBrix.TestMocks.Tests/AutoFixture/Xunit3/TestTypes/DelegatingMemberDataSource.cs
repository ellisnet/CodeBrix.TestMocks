using CodeBrix.TestMocks.AutoFixture.Xunit3.Internal;
using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class DelegatingMemberDataSource : MemberDataSource
{
    public DelegatingMemberDataSource(Type type, string name, params object[] arguments)
        : base(type, name, arguments)
    {
    }

    public DataSource GetSource() => this.Source;
}