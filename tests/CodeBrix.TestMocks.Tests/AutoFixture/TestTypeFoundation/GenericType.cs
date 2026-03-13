using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class GenericType<T>
    where T : class
{
    public GenericType(T t)
    {
        if (t == null)
        {
            throw new ArgumentNullException(nameof(t));
        }

        this.Value = t;
    }

    private T Value { get; }
}