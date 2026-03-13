using System.Collections;
using System.Collections.Generic;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class ClassWithNullTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return null;
        yield return null;
        yield return null;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}