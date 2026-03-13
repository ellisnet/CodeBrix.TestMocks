namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock.TestTypes; //was previously: namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public interface IInterfaceWithIndexer
{
    int this[int index] { get; }
}