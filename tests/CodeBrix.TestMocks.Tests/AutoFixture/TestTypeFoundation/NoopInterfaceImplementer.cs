namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class NoopInterfaceImplementer : IInterface
{
    public object MakeIt(object obj)
    {
        return obj;
    }
}