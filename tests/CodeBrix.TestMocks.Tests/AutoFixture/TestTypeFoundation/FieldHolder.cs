using System.Reflection;

namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class FieldHolder<T>
{
    public T Field;

    public static FieldInfo GetField()
    {
        return typeof(FieldHolder<T>)
            .GetRuntimeField(nameof(Field));
    }
}