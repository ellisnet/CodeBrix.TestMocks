using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeBrix.TestMocks.Tests.AutoFixture.DataAnnotations;  //was previously: namespace AutoFixtureUnitTest.DataAnnotations;

public class StringLengthValidatedPropertyHolder<T>
{
    [StringLength(5)]
    public T Property { get; set; }

    public static PropertyInfo GetProperty()
    {
        return typeof(StringLengthValidatedPropertyHolder<T>)
            .GetProperty(nameof(Property));
    }
}