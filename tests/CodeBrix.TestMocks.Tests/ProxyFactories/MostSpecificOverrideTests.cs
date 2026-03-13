using CodeBrix.TestMocks.Mocking;
using System;
using System.Reflection;
using Xunit;

namespace CodeBrix.TestMocks.Tests.ProxyFactories;

public class MostSpecificOverrideTests
{
    [Theory]
    [InlineData(typeof(IA), "Method", typeof(IA), typeof(IA), "Method")]
    [InlineData(typeof(IA), "Method", typeof(CA), typeof(IA), "Method")]
    [InlineData(typeof(IA), "Method", typeof(CAimpl), typeof(CAimpl), "Method")]
    [InlineData(typeof(IA), "Method", typeof(IBoverride), typeof(IBoverride), "CodeBrix.TestMocks.Tests.ProxyFactories.MostSpecificOverrideTests.IA.Method")]
    [InlineData(typeof(IA), "Method", typeof(IBnew), typeof(IA), "Method")]
    [InlineData(typeof(IA), "Method", typeof(ICfromBoverride), typeof(ICfromBoverride), "CodeBrix.TestMocks.Tests.ProxyFactories.MostSpecificOverrideTests.IA.Method")]
    [InlineData(typeof(IA), "Method", typeof(ICfromBnew), typeof(ICfromBnew), "CodeBrix.TestMocks.Tests.ProxyFactories.MostSpecificOverrideTests.IA.Method")]
    [InlineData(typeof(IBnew), "Method", typeof(ICfromBnew), typeof(ICfromBnew), "CodeBrix.TestMocks.Tests.ProxyFactories.MostSpecificOverrideTests.IBnew.Method")]
    public void Finds_correct_most_specific_override(Type declarationType, string declarationName, Type proxiedType, Type overrideType, string overrideName)
    {
        var expected = overrideType.GetMethod(overrideName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        var proxy = typeof(Mock).GetMethod("Of", BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null).MakeGenericMethod(proxiedType).Invoke(null, null);
        var proxyType = proxy.GetType();
        var actual = CastleProxyFactory.FindMostSpecificOverride(
            declaration: declarationType.GetMethod(declarationName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
            proxyType);
        Assert.Same(expected, actual);
    }

    public interface IA
    {
        string Method() => "Method in IA";
    }

    public class CA : IA
    {
    }

    public class CAimpl : IA
    {
        public string Method() => "Method in CAimpl";
    }

    public interface IBoverride : IA
    {
        string IA.Method() => "IA.Method in IBoverride";
    }

    public interface IBnew : IA
    {
        new string Method() => "Method in IBnew";
    }

    public interface ICfromBoverride : IBoverride
    {
        string IA.Method() => "IA.Method in ICfromBoverride";
    }

    public interface ICfromBnew : IBnew
    {
        string IBnew.Method() => "IBnewMethod in ICfromBnew";
        string IA.Method() => "IA.Method in ICfromBnew";
    }
}
