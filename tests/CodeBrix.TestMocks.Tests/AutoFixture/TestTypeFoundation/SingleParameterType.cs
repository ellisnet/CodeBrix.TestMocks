namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class SingleParameterType<T>
{
    public SingleParameterType(T parameter)
    {
        this.Parameter = parameter;
    }

    public T Parameter { get; private set; }
}