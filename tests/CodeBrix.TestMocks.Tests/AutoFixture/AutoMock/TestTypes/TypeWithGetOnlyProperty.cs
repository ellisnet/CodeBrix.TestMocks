namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public class TypeWithGetOnlyProperty
{
    public string GetOnlyProperty
    {
        get { return string.Empty; }
    }
}