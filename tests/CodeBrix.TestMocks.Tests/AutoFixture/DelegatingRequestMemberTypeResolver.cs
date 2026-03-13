using System;
using CodeBrix.TestMocks.AutoFixture;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

internal class DelegatingRequestMemberTypeResolver : IRequestMemberTypeResolver
{
    public DelegatingRequestMemberTypeResolver()
    {
        this.OnTryGetMemberType = r => null;
    }

    public bool TryGetMemberType(object request, out Type memberType)
    {
        memberType = this.OnTryGetMemberType(request);
        return memberType != null;
    }

    internal Func<object, Type> OnTryGetMemberType { get; set; }
}