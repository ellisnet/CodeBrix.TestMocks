using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class DelegatingSpecimenBuilderTransformation : ISpecimenBuilderTransformation
{
    public DelegatingSpecimenBuilderTransformation()
    {
        this.OnTransform = b => (ISpecimenBuilderNode)b;
    }

    public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
    {
        return this.OnTransform(builder);
    }

    internal Func<ISpecimenBuilder, ISpecimenBuilderNode> OnTransform { get; set; }
}