namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public interface IDerivedInterfaceWithProperty : IInterfaceWithProperty
{
    string DerivedProperty { get; set; }
}