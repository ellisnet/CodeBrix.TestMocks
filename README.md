# CodeBrix.TestMocks

A single-package .NET testing library that provides **mocking** and **auto-generated test data**, with xUnit v3 integration built in — everything you need to write thorough unit tests from one package reference. CodeBrix.TestMocks is provided as a .NET 10 library and associated `CodeBrix.TestMocks.ApacheLicenseForever` NuGet package.

CodeBrix.TestMocks supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

| | |
| --- | --- |
| **NuGet** | `CodeBrix.TestMocks.ApacheLicenseForever` |
| **Target framework** | .NET 10+ |
| **Test framework** | xUnit v3 |
| **License** | Apache License 2.0 |

The package id carries the `ApacheLicenseForever` suffix as a commitment: it is published under the Apache License 2.0 and will not be switched to another license. The assembly and namespace root are plain `CodeBrix.TestMocks`.

## Installation

```
dotnet add package CodeBrix.TestMocks.ApacheLicenseForever
```

This package brings in only what its data attributes compile against, so your test project also needs the xUnit v3 framework and a runner:

```xml
<PackageReference Include="CodeBrix.TestMocks.ApacheLicenseForever" Version="..." />
<PackageReference Include="xunit.v3" Version="..." />
<PackageReference Include="xunit.runner.visualstudio" Version="...">
  <PrivateAssets>all</PrivateAssets>
</PackageReference>
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="..." />
```

On the .NET 10 SDK, add a `global.json` beside your solution so that `dotnet test` runs xUnit v3 through Microsoft Testing Platform:

```json
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

Without it, `dotnet test` stops with *"Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and later."* Note also that `--nologo` is a VSTest-only switch — passing it in this mode reports "Zero tests ran" without running anything. Use `dotnet test -- --no-banner` instead.

No assertion library is included; use xUnit's `Assert` or a fluent assertion package of your choice.

## CodeBrix.TestMocks supports:

* Mock creation, setup and verification for interfaces and for the virtual members of a class - `new Mock<T>()`, `Mock.Of<T>()`, `Setup(...)`, `Returns(...)`, `Verify(...)`
* Argument matchers - `It.IsAny<T>()`, `It.Is<T>(x => ...)`, `It.IsInRange(1, 100, Range.Inclusive)`
* Call-count assertions - `Times.Once()`, `Times.Never()`, `Times.Exactly(n)`, `Times.AtLeastOnce()`
* Callbacks, call sequences and property setup - `Callback<T>(...)`, `SetupSequence(...)`, `SetupProperty(...)`, `SetupAllProperties()`
* Loose mocks by default, with `MockBehavior.Strict` when unmatched calls should throw
* Additional interfaces on an existing mock - `mock.As<IDisposable>()`
* Async setups - `ReturnsAsync(...)` and `ThrowsAsync(...)`
* Auto-generated test data and whole object graphs - `Fixture`, `Create<T>()`, `CreateMany<T>()`, `Freeze<T>()`, and the `Build<T>().With(...).Without(...).Create()` customization chain
* Auto-mocking of constructor dependencies through `AutoMockCustomization`, so a system under test can be built with all of its dependencies mocked and no fixture plumbing
* xUnit v3 data attributes for data-driven tests - `[AutoData]`, `[InlineAutoData]`, `[MemberAutoData]`, `[AutoMockData]`, `[InlineAutoMockData]` and `[Frozen]`
* Deriving your own data attributes from `AutoDataAttribute` or `InlineAutoDataAttribute` for project-wide conventions
* The proxy generator the mocking API is built on, usable directly for aspect-style interception (logging, timing, retry, lazy loading) - `ProxyGenerator`, `IInterceptor`

Where those live:

| Namespace | Purpose |
| --- | --- |
| `CodeBrix.TestMocks.Mocking` | Mock creation, setup and verification |
| `CodeBrix.TestMocks.AutoFixture` | Auto-generated test data and object graphs |
| `CodeBrix.TestMocks.AutoFixture.AutoMock` | Auto-mocking of constructor dependencies |
| `CodeBrix.TestMocks.AutoFixture.Xunit3` | xUnit v3 attributes for data-driven tests |
| `CodeBrix.TestMocks.AutoFixture.AutoMock.Data` | Ready-made `[AutoMockData]` and `[InlineAutoMockData]` attributes |

Every public namespace begins with `CodeBrix.TestMocks`.

## Sample Code

The examples below use these types:

```csharp
public interface IOrderRepository
{
    Order GetById(int id);
    Task<Order> GetByIdAsync(int id);
    void Save(Order order);
    void Delete(int id);
}

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal Total { get; set; }
    public bool IsProcessed { get; set; }
}
```

### Mocking

Create a mock of an interface (or the virtual members of a class), set up the behavior you need, and verify how your code used it.

```csharp
using CodeBrix.TestMocks.Mocking;
using Xunit;

[Fact]
public void ProcessOrder_SavesTheOrder()
{
    var mockRepo = new Mock<IOrderRepository>();
    var order = new Order { Id = 42, CustomerName = "Alice" };

    mockRepo.Setup(r => r.GetById(42)).Returns(order);

    // mockRepo.Object is the IOrderRepository your code consumes
    var sut = new OrderProcessor(mockRepo.Object, Mock.Of<IEmailService>());
    sut.Process(42);

    mockRepo.Verify(r => r.Save(It.Is<Order>(o => o.Id == 42)), Times.Once());
    mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never());
}
```

Common building blocks:

- **Argument matchers** — `It.IsAny<T>()`, `It.Is<T>(x => ...)`, `It.IsInRange(1, 100, Range.Inclusive)`
- **Async results** — `.ReturnsAsync(order)` and `.ThrowsAsync(new InvalidOperationException())`
- **Call counts** — `Times.Once()`, `Times.Never()`, `Times.Exactly(3)`, `Times.AtLeastOnce()`
- **Callbacks** — `.Callback<Order>(o => saved.Add(o))` to capture arguments
- **Sequences** — `SetupSequence(...)` to return a different value on each call
- **Properties** — `SetupProperty(l => l.Level, LogLevel.Info)` or `SetupAllProperties()`
- **Behavior** — mocks are loose by default (unmatched calls return defaults); pass `MockBehavior.Strict` to make them throw instead
- **Extra interfaces** — `mock.As<IDisposable>().Setup(d => d.Dispose())`

### Test data

`Fixture` generates anonymous but valid values for any type, so tests only state the values they actually care about.

```csharp
using CodeBrix.TestMocks.AutoFixture;

var fixture = new Fixture();

string name = fixture.Create<string>();
Order order = fixture.Create<Order>();               // every property populated
IEnumerable<Order> orders = fixture.CreateMany<Order>(5);

// Freeze: the same instance is reused everywhere the fixture resolves that type
var frozen = fixture.Freeze<Order>();

// Build: override just the properties that matter to this test
var custom = fixture.Build<Order>()
                    .With(o => o.CustomerName, "Alice")
                    .Without(o => o.IsProcessed)
                    .Create();
```

### Auto-mocking

`AutoMockCustomization` connects the two halves: when the fixture meets an interface or abstract class it cannot construct, it supplies a mock instead. That means a system under test can be built with all of its dependencies mocked, without any fixture plumbing.

```csharp
[Theory, AutoMockData]
public void Process_WhenOrderExists_SavesAndNotifies(
    [Frozen] Mock<IOrderRepository> mockRepo,
    [Frozen] Mock<IEmailService> mockEmail,
    Order order,
    OrderProcessor sut)          // constructed with the frozen mocks above
{
    mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);

    sut.Process(order.Id);

    Assert.True(order.IsProcessed);
    mockRepo.Verify(r => r.Save(order), Times.Once());
    mockEmail.Verify(
        e => e.SendEmail(order.CustomerName, "Processed", It.IsAny<string>()),
        Times.Once());
}
```

`[Frozen]` is what makes this work: it guarantees the mock you configure is the same instance the system under test received. Without it, your `Verify` fails for no visible reason.

## xUnit v3 attributes

| Attribute | What it does |
| --- | --- |
| `[AutoData]` | Auto-generates every test parameter |
| `[InlineAutoData(...)]` | Takes the leading parameters explicitly, auto-generates the rest |
| `[MemberAutoData(nameof(...))]` | Takes the leading parameters from member data, auto-generates the rest |
| `[AutoMockData]` | Like `[AutoData]`, with auto-mocking enabled |
| `[InlineAutoMockData(...)]` | Like `[InlineAutoData]`, with auto-mocking enabled |
| `[Frozen]` | Pins a parameter so the same instance is injected everywhere |

Use them on `[Theory]` methods:

```csharp
[Theory, AutoData]
public void CustomerName_IsPopulated(Order order)
    => Assert.False(string.IsNullOrEmpty(order.CustomerName));

[Theory]
[InlineAutoData("Alice")]
[InlineAutoData("Bob")]
public void Greeting_ContainsName(string name, int orderId)
    => Assert.Contains(name, $"Hello {name}, order #{orderId} is ready.");
```

For project-wide conventions, derive your own attribute from `AutoDataAttribute` or `InlineAutoDataAttribute` and pass a configured fixture to the base constructor.

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete API reference and usage guide written for AI coding agents - point your agent at that file when it is writing code against this library. It is the most thorough documentation available, and it reads perfectly well for humans; it is also [readable on GitHub](https://github.com/ellisnet/CodeBrix.TestMocks/blob/main/AGENT-README.txt).

Additional sample code and usage examples are available in the `CodeBrix.TestMocks.Tests` project, which doubles as executable documentation with roughly one file per feature area:
https://github.com/ellisnet/CodeBrix.TestMocks/tree/main/tests/CodeBrix.TestMocks.Tests

## License

CodeBrix.TestMocks is licensed under the Apache License 2.0 — see the [LICENSE](https://github.com/ellisnet/CodeBrix.TestMocks/blob/main/LICENSE) file.

For licensing and provenance information about the open source code included in this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.TestMocks/blob/main/THIRD-PARTY-NOTICES.txt).
