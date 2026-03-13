using System;
using System.Reflection;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class DelegatingMethodFactory : IMethodFactory
{
    public DelegatingMethodFactory()
    {
        this.OnCreate = m => null;
    }

    public IMethod Create(MethodInfo methodInfo)
    {
        return this.OnCreate(methodInfo);
    }

    internal Func<MethodInfo, IMethod> OnCreate { get; set; }
}