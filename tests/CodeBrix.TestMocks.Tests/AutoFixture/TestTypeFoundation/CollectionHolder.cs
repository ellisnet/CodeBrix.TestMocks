using System.Collections.Generic;

namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class CollectionHolder<T>
{
    public CollectionHolder()
    {
        this.Collection = new List<T>();
    }

    public ICollection<T> Collection { get; private set; }
}