namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public abstract class AbstractGenericType<T>
{
    protected AbstractGenericType(T t)
    {
        this.Value = t;
    }

    public T Value { get; }
}