using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

internal class ProvidedAttribute
{
    public ProvidedAttribute(Attribute attribute, bool inherited)
    {
        this.Attribute = attribute;
        this.Inherited = inherited;
    }

    public Attribute Attribute { get; }

    public bool Inherited { get; }
}