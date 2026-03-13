using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

internal class DelegatingTracingBuilder : TracingBuilder
{
    public DelegatingTracingBuilder()
        : this(new DelegatingSpecimenBuilder())
    {
    }

    public DelegatingTracingBuilder(ISpecimenBuilder builder)
        : base(builder)
    {
    }

    internal void RaiseSpecimenCreated(SpecimenCreatedEventArgs e)
    {
        this.OnSpecimenCreated(e);
    }

    internal void RaiseSpecimenRequested(RequestTraceEventArgs e)
    {
        this.OnSpecimenRequested(e);
    }
}