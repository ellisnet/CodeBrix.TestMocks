using System;

namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public abstract class AbstractTypeWithNonDefaultConstructor<T>
{
    protected AbstractTypeWithNonDefaultConstructor(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.Property = value;
    }

    public T Property { get; }
}