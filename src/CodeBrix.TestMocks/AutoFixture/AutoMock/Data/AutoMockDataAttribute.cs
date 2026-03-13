using System;
using System.Diagnostics.CodeAnalysis;
using CodeBrix.TestMocks.AutoFixture.Xunit3;

namespace CodeBrix.TestMocks.AutoFixture.AutoMock.Data;

/// <summary>
/// Provides auto-generated data specimens with automatic mocking enabled.
/// This is an <see cref="AutoDataAttribute"/> that configures the <see cref="Fixture"/>
/// with <see cref="AutoMockCustomization"/> (with <c>ConfigureMembers = true</c>),
/// so that interface and abstract class dependencies are automatically mocked.
/// </summary>
/// <example>
///   <code>
///     [Theory, AutoMockData]
///     public void MyTest(
///         [Frozen] Mock&lt;IMyService&gt; mockService,
///         MyClass sut)
///     {
///         mockService.Setup(s =&gt; s.DoWork()).Returns(true);
///         Assert.True(sut.Run());
///     }
///   </code>
/// </example>
[AttributeUsage(AttributeTargets.Method)]
[CLSCompliant(false)]
[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
    Justification = "This attribute is the root of a potential attribute hierarchy.")]
public class AutoMockDataAttribute : AutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMockDataAttribute"/> class.
    /// The fixture is configured with <see cref="AutoMockCustomization"/> with
    /// <c>ConfigureMembers = true</c>.
    /// </summary>
    public AutoMockDataAttribute()
        : base(CreateFixture)
    {
    }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });
        return fixture;
    }
}
