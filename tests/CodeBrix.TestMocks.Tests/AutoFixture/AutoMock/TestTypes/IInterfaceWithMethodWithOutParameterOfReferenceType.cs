namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public interface IInterfaceWithMethodWithOutParameterOfReferenceType
{
    string Method(out string s);
}