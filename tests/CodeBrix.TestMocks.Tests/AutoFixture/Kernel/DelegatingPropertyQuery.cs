using System;
using System.Collections.Generic;
using System.Reflection;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class DelegatingPropertyQuery : IPropertyQuery
{
    public DelegatingPropertyQuery()
    {
        this.OnSelectProperties = t => t.GetTypeInfo().GetProperties();
    }

    public IEnumerable<PropertyInfo> SelectProperties(Type type)
        => this.OnSelectProperties?.Invoke(type);

    internal Func<Type, IEnumerable<PropertyInfo>> OnSelectProperties { get; set; }
}