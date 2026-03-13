// Copyright (c) 2007, Clarius Consulting, Manas Technology Solutions, InSTEDD, and Contributors.
// All rights reserved. Licensed under the BSD 3-Clause License; see License.txt.

using CodeBrix.TestMocks.Mocking;

using Xunit;

namespace CodeBrix.TestMocks.Tests;

public class RecordsTests
{
    [Fact]
    public void Can_mock_EmptyRecord()
    {
        _ = new Mock<EmptyRecord>().Object;
    }

    [Fact]
    public void Can_mock_DerivedEmptyRecord()
    {
        _ = new Mock<DerivedEmptyRecord>().Object;
    }

    public record EmptyRecord
    {
    }

    public record DerivedEmptyRecord : EmptyRecord
    {
    }
}