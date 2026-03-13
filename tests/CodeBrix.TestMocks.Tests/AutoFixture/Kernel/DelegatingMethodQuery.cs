using System;
using System.Collections.Generic;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class DelegatingMethodQuery : IMethodQuery
{
    public DelegatingMethodQuery()
    {
        this.OnSelectMethods = t => Enumerable.Empty<IMethod>();
    }

    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        return this.OnSelectMethods(type);
    }

    internal Func<Type, IEnumerable<IMethod>> OnSelectMethods { get; set; }
}