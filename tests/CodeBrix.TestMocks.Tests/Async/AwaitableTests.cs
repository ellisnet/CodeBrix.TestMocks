using System.Threading.Tasks;

using CodeBrix.TestMocks.Mocking.Async;

using Xunit;

namespace CodeBrix.TestMocks.Tests.Async;

public class AwaitableTests
{
    [Fact]
    public void TryGetResultRecursive_is_recursive()
    {
        const int expectedResult = 42;
        var obj = Task.FromResult(Task.FromResult(expectedResult));
        var result = Awaitable.TryGetResultRecursive(obj);
        Assert.Equal(expectedResult, result);
    }
}
