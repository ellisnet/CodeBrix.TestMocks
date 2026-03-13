using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public static class GuardedStaticMethodOnStaticTypeHost
{
    public static void Method(object argument)
    {
        if (argument == null) throw new ArgumentNullException(nameof(argument));
    }
}