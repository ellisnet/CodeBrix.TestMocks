using CodeBrix.TestMocks.AutoFixture.Xunit3.Internal;
using System;
using System.Collections.Generic;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Xunit3.TestTypes; //was previously: namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class DelegatingDataSource : DataSource
{
    public IEnumerable<object[]> TestData { get; set; } = Array.Empty<object[]>();

    protected override IEnumerable<object[]> GetData()
    {
        return this.TestData;
    }
}