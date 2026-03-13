// Copyright (c) 2007, Clarius Consulting, Manas Technology Solutions, InSTEDD, and Contributors.
// All rights reserved. Licensed under the BSD 3-Clause License; see License.txt.

using System;

using CodeBrix.TestMocks.Mocking;

using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete

namespace CodeBrix.TestMocks.Tests;

public class OccurrenceTests
{
    [Fact]
    public void OnceThrowsOnSecondCall()
    {
        var mock = new Mock<IFoo>();
        mock.Setup(foo => foo.Execute("ping"))
            .Returns("ack")
            .AtMostOnce();

        Assert.Equal("ack", mock.Object.Execute("ping"));
        MockException mex = Assert.Throws<MockException>(() => mock.Object.Execute("ping"));
        Assert.Equal(MockExceptionReasons.IncorrectNumberOfCalls, mex.Reasons);
    }

    [Fact]
    public void RepeatThrowsOnNPlusOneCall()
    {
        var repeat = 5;
        var mock = new Mock<IFoo>();
        mock.Setup(foo => foo.Execute("ping"))
            .Returns("ack")
            .AtMost(5);

        var calls = 0;
        MockException mex = Assert.Throws<MockException>(() =>
        {
            while (calls <= repeat + 1)
            {
                mock.Object.Execute("ping");
                calls++;
            }

            Assert.Fail("should fail on two calls");
        });

        Assert.Equal(MockExceptionReasons.IncorrectNumberOfCalls, mex.Reasons);
        Assert.Equal(calls, repeat);
    }

    [Fact]
    public void CallsThatThrowExceptionStillCountAsCalls()
    {
        var mock = new Mock<IFoo>();
        mock.Setup(f => f.Submit()).Throws<InvalidOperationException>().AtMostOnce();

        var firstException = Record.Exception(() => mock.Object.Submit());
        var secondException = Record.Exception(() => mock.Object.Submit());

        // The first call should trigger the exception specified in the setup:
        Assert.IsType<InvalidOperationException>(firstException);

        // The second call should trigger a MockException since the max call count
        // was exceeded:
        Assert.IsType<MockException>(secondException);
    }

    public interface IFoo
    {
        int Value { get; set; }
        int Echo(int value);
        void Submit();
        string Execute(string command);
    }
}
