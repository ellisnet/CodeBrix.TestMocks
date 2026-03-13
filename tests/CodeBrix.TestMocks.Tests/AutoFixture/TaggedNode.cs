using System.Collections.Generic;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class TaggedNode : CompositeSpecimenBuilder
{
    public TaggedNode(object tag, params ISpecimenBuilder[] builders)
        : base(builders)
    {
        this.Tag = tag;
    }

    public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
    {
        return new TaggedNode(this.Tag, builders.ToArray());
    }

    public object Tag { get; }
}