using System;
using System.Diagnostics.CodeAnalysis;
using CodeBrix.TestMocks.AutoFixture.Xunit3;

namespace CodeBrix.TestMocks.AutoFixture.AutoMock.Data;

/// <summary>
/// Provides a data source for a data theory, with some data coming from inline values
/// and the rest auto-generated with automatic mocking enabled.
/// This is an <see cref="InlineAutoDataAttribute"/> that configures the <see cref="Fixture"/>
/// with <see cref="AutoMockCustomization"/> (with <c>ConfigureMembers = true</c>),
/// so that interface and abstract class dependencies are automatically mocked.
/// </summary>
/// <example>
///   <code>
///     [Theory]
///     [InlineAutoMockData(42)]
///     [InlineAutoMockData(99)]
///     public void MyTest(
///         int orderId,
///         [Frozen] Mock&lt;IMyService&gt; mockService,
///         MyClass sut)
///     {
///         mockService.Setup(s =&gt; s.GetById(orderId)).Returns(new Order());
///         sut.Process(orderId);
///         mockService.Verify(s =&gt; s.GetById(orderId), Times.Once());
///     }
///   </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
[CLSCompliant(false)]
[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
    Justification = "This attribute is the root of a potential attribute hierarchy.")]
public class InlineAutoMockDataAttribute : InlineAutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InlineAutoMockDataAttribute"/> class.
    /// The fixture is configured with <see cref="AutoMockCustomization"/> with
    /// <c>ConfigureMembers = true</c>.
    /// </summary>
    /// <param name="values">The data values to pass to the theory.</param>
    public InlineAutoMockDataAttribute(params object[] values)
        : base(CreateFixture, values)
    {
    }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });
        return fixture;
    }
}
