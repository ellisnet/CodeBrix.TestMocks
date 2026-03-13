using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeBrix.TestMocks.Tests.AutoFixture.DataAnnotations;  //was previously: namespace AutoFixtureUnitTest.DataAnnotations;

public class StringLengthValidatedFieldHolder<T>
{
    [StringLength(5)]
    public T Field;

    public static FieldInfo GetField()
    {
        return typeof(StringLengthValidatedFieldHolder<T>)
            .GetField(nameof(Field));
    }
}