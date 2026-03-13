using CodeBrix.TestMocks.Mocking;

using Xunit;

namespace CodeBrix.TestMocks.Tests.Regressions;

public class FluentMockIssuesTests
{
    public interface IOne
    {
        ITwo Two { get; }
    }

    public interface ITwo
    {
        IThree Three { get; }
    }

    public interface IThree
    {
        ITwo LoopBack { get; }
        string SomeString { get; }
    }

    [Fact]
    public void CyclesInThePropertyGraphAreHandled()
    {
        var foo = new Mock<IOne> { DefaultValue = DefaultValue.Mock };
        foo.SetupGet(m => m.Two.Three.SomeString).Returns("blah");

        // the default value of the loopback property is mocked
        Assert.NotNull(foo.Object.Two.Three.LoopBack);
        Assert.NotSame(foo.Object.Two, foo.Object.Two.Three.LoopBack);
    }
}
