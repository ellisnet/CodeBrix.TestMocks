using CodeBrix.TestMocks.AutoFixture.Xunit3;
using CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation;
using System.Collections.Generic;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class SampleTestType
{
    public void TestMethodWithoutParameters()
    {
    }

    public void TestMethodWithSingleParameter(string value)
    {
    }

    public void TestMethodWithMultipleParameters(string a, int b, double c)
    {
    }

    public void TestMethodWithReferenceTypeParameter(string a, int b, PropertyHolder<string> c)
    {
    }

    public void TestMethodWithRecordTypeParameter(string a, int b, RecordType<string> c)
    {
    }

    public void TestMethodWithCollectionParameter(string a, int b, IEnumerable<int> c)
    {
    }

    public void TestMethodWithCustomizedParameter([Frozen] string a, int b, PropertyHolder<string> c)
    {
    }

    public void TestMethodWithMultipleCustomizations([Frozen] string a, [Frozen] int b, [Greedy][Frozen] PropertyHolder<string> c)
    {
    }
}