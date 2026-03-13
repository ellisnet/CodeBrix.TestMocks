namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public interface IInterfaceWithPropertyWithCircularDependency
{
    IInterfaceWithPropertyWithCircularDependency Property { get; set; }
}