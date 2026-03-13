namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public class TypeWithPrivateProperty
{
    public TypeWithPrivateProperty()
    {
        this.PrivateProperty = "Awesome string";
    }

    // ReSharper disable UnusedAutoPropertyAccessor.Local
    private string PrivateProperty { get; set; }
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}