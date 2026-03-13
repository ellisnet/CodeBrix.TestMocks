namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class ReadOnlyPropertyHolder<T>
{
    public T Property { get; private set; }
}