using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

internal class FixedParameterBuilder<T> : FilteringSpecimenBuilder
{
    public FixedParameterBuilder(string name, T value)
        : base(new FixedBuilder(value), new ParameterSpecification(typeof(T), name))
    {
    }
}