namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public interface IInterfaceWithRefMethod
{
    string Method(ref string s);
}