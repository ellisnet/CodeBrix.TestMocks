namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public interface IDerivedInterfaceOfDerivedInterfaceWithProperty : IDerivedInterfaceWithProperty
{
    string DerivedDerivedProperty { get; set; }
}