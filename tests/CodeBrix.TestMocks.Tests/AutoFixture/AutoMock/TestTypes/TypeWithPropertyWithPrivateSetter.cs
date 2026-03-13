namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public class TypeWithPropertyWithPrivateSetter
{
    public TypeWithPropertyWithPrivateSetter()
    {
        this.PropertyWithPrivateSetter = "Awesome string";
    }

    public string PropertyWithPrivateSetter { get; private set; }
}