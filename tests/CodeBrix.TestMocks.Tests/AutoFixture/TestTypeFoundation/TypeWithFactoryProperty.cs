namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class TypeWithFactoryProperty
{
    private TypeWithFactoryProperty()
    {
    }

    public static TypeWithFactoryProperty Factory
    {
        get
        {
            return new TypeWithFactoryProperty();
        }
    }
}