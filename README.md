# CodeBrix.TestMocks

A comprehensive, single-package testing library for .NET that provides **mocking** and **auto-generated test data** capabilities — everything you need to write thorough, maintainable unit tests.

CodeBrix.TestMocks is a fork of the popular [Moq](https://github.com/devlooped/moq) and [AutoFixture](https://github.com/AutoFixture/AutoFixture) open source libraries, combined into a single unified package with built-in xUnit v3 integration. It is published under the Apache License 2.0 - and available as the `CodeBrix.TestMocks.ApacheLicenseForever` NuGet package - with a commitment to never switch the Nuget package to another license.

| | |
| --- | --- |
| **NuGet** | `CodeBrix.TestMocks.ApacheLicenseForever` |
| **Target Framework** | .NET 10+ |
| **License** | Apache License 2.0 |
| **Test Framework** | xUnit v3 |

## Installation

Install the NuGet package in your test project:

```
dotnet add package CodeBrix.TestMocks.ApacheLicenseForever
```

## Namespaces

| Namespace | Purpose |
| --- | --- |
| `CodeBrix.TestMocks.Mocking` | Mock creation, setup, and verification (forked from Moq) |
| `CodeBrix.TestMocks.AutoFixture` | Auto-generated test data and specimen creation (forked from AutoFixture) |
| `CodeBrix.TestMocks.AutoFixture.AutoMock` | Integration between AutoFixture and the mocking framework |
| `CodeBrix.TestMocks.AutoFixture.Xunit3` | xUnit v3 attributes for data-driven tests with auto-generated data |
| `CodeBrix.TestMocks.AutoFixture.AutoMock.Data` | Ready-to-use `[AutoMockData]` and `[InlineAutoMockData]` xUnit v3 attributes |

---

## Mocking — `CodeBrix.TestMocks.Mocking`

The mocking API lets you create mock implementations of interfaces (and virtual members of classes), set up expected behaviors, and verify that your code interacted with dependencies as expected.

### Interfaces used in examples

```csharp
public interface IOrderRepository
{
    Order GetById(int id);
    Task<Order> GetByIdAsync(int id);
    IList<Order> GetByCustomer(string customerName);
    void Save(Order order);
    Task SaveAsync(Order order);
    void Delete(int id);
    bool Exists(int id);
}

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
    Task SendEmailAsync(string to, string subject, string body);
}

public interface ILogger
{
    void Log(string message);
    LogLevel Level { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal Total { get; set; }
    public bool IsProcessed { get; set; }
}

public enum LogLevel { Debug, Info, Warning, Error }
```

### Basic setup and returns

```csharp
using CodeBrix.TestMocks.Mocking;
using Xunit;

public class OrderServiceTests
{
    [Fact]
    public void GetOrder_ReturnsOrderFromRepository()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var expectedOrder = new Order { Id = 42, CustomerName = "Alice", Total = 99.95m };

        mockRepo.Setup(r => r.GetById(42)).Returns(expectedOrder);

        // Act — use mockRepo.Object to get the mocked interface instance
        IOrderRepository repo = mockRepo.Object;
        var result = repo.GetById(42);

        // Assert
        Assert.Equal("Alice", result.CustomerName);
        Assert.Equal(99.95m, result.Total);
    }
}
```

### Argument matchers with `It`

Use the `It` class to match arguments by condition rather than exact value:

```csharp
[Fact]
public void Save_AcceptsAnyOrder()
{
    var mockRepo = new Mock<IOrderRepository>();

    // Match any Order argument
    mockRepo.Setup(r => r.Save(It.IsAny<Order>()));

    // Match by predicate
    mockRepo.Setup(r => r.GetById(It.Is<int>(id => id > 0)))
            .Returns(new Order { Id = 1 });

    // Match a value in a range
    mockRepo.Setup(r => r.GetById(It.IsInRange(1, 100, Range.Inclusive)))
            .Returns(new Order { Id = 50 });

    var repo = mockRepo.Object;
    repo.Save(new Order { Id = 99, CustomerName = "Bob" }); // matches It.IsAny<Order>()
    var order = repo.GetById(50);                             // matches the range setup

    Assert.NotNull(order);
}
```

### Callbacks

Execute custom logic when a mocked method is called:

```csharp
[Fact]
public void Save_TracksAllSavedOrders()
{
    var mockRepo = new Mock<IOrderRepository>();
    var savedOrders = new List<Order>();

    mockRepo.Setup(r => r.Save(It.IsAny<Order>()))
            .Callback<Order>(order => savedOrders.Add(order));

    var repo = mockRepo.Object;
    repo.Save(new Order { Id = 1, CustomerName = "Alice" });
    repo.Save(new Order { Id = 2, CustomerName = "Bob" });

    Assert.Equal(2, savedOrders.Count);
    Assert.Equal("Bob", savedOrders[1].CustomerName);
}
```

### Verification

Verify that methods were called with the expected arguments and call counts:

```csharp
[Fact]
public void ProcessOrder_SendsEmailAndSaves()
{
    var mockRepo = new Mock<IOrderRepository>();
    var mockEmail = new Mock<IEmailService>();

    var order = new Order { Id = 1, CustomerName = "Alice" };

    // Act — your service under test would use these mocks
    mockRepo.Object.Save(order);
    mockEmail.Object.SendEmail("alice@example.com", "Order Confirmation", "Your order #1 is confirmed.");

    // Verify the repository Save was called exactly once with the correct order
    mockRepo.Verify(r => r.Save(It.Is<Order>(o => o.Id == 1)), Times.Once());

    // Verify email was sent
    mockEmail.Verify(e => e.SendEmail(
        It.IsAny<string>(),
        It.Is<string>(s => s.Contains("Confirmation")),
        It.IsAny<string>()), Times.Once());

    // Verify Delete was never called
    mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never());
}
```

### Async method mocking

Use `ReturnsAsync` and `ThrowsAsync` for `Task`-returning methods:

```csharp
[Fact]
public async Task GetOrderAsync_ReturnsOrder()
{
    var mockRepo = new Mock<IOrderRepository>();

    mockRepo.Setup(r => r.GetByIdAsync(42))
            .ReturnsAsync(new Order { Id = 42, CustomerName = "Alice" });

    var result = await mockRepo.Object.GetByIdAsync(42);

    Assert.Equal("Alice", result.CustomerName);
}

[Fact]
public async Task SaveAsync_ThrowsOnFailure()
{
    var mockRepo = new Mock<IOrderRepository>();

    mockRepo.Setup(r => r.SaveAsync(It.IsAny<Order>()))
            .ThrowsAsync(new InvalidOperationException("Database unavailable"));

    await Assert.ThrowsAsync<InvalidOperationException>(
        () => mockRepo.Object.SaveAsync(new Order()));
}
```

### Strict vs. Loose behavior

```csharp
[Fact]
public void StrictMock_ThrowsForUnexpectedCalls()
{
    // Strict: any call without a matching setup throws MockException
    var mockRepo = new Mock<IOrderRepository>(MockBehavior.Strict);
    mockRepo.Setup(r => r.GetById(1)).Returns(new Order { Id = 1 });

    var order = mockRepo.Object.GetById(1); // OK — has a setup
    Assert.NotNull(order);

    // mockRepo.Object.GetById(999); // would throw MockException — no setup for id 999
}

[Fact]
public void LooseMock_ReturnsDefaultForUnexpectedCalls()
{
    // Loose (default): unexpected calls return default values
    var mockRepo = new Mock<IOrderRepository>(); // MockBehavior.Loose is the default

    var order = mockRepo.Object.GetById(999); // returns null (default for reference type)
    Assert.Null(order);

    var exists = mockRepo.Object.Exists(1); // returns false (default for bool)
    Assert.False(exists);
}
```

### Property setup and stubbing

```csharp
[Fact]
public void SetupProperty_TracksValueChanges()
{
    var mockLogger = new Mock<ILogger>();

    // SetupProperty enables get/set tracking (stubbing)
    mockLogger.SetupProperty(l => l.Level, LogLevel.Info);

    var logger = mockLogger.Object;
    Assert.Equal(LogLevel.Info, logger.Level); // initial value

    logger.Level = LogLevel.Error;
    Assert.Equal(LogLevel.Error, logger.Level); // value is tracked

    // SetupAllProperties stubs every property at once
    var mockLogger2 = new Mock<ILogger>();
    mockLogger2.SetupAllProperties();
}
```

### Setup sequences

Return different values on consecutive calls:

```csharp
[Fact]
public void SetupSequence_ReturnsDifferentValuesPerCall()
{
    var mockRepo = new Mock<IOrderRepository>();

    mockRepo.SetupSequence(r => r.GetById(It.IsAny<int>()))
            .Returns(new Order { Id = 1, CustomerName = "First" })
            .Returns(new Order { Id = 2, CustomerName = "Second" })
            .Throws(new InvalidOperationException("No more orders"));

    var repo = mockRepo.Object;

    Assert.Equal("First", repo.GetById(1).CustomerName);
    Assert.Equal("Second", repo.GetById(2).CustomerName);
    Assert.Throws<InvalidOperationException>(() => repo.GetById(3));
}
```

### Throwing exceptions

```csharp
[Fact]
public void Setup_ThrowsException()
{
    var mockRepo = new Mock<IOrderRepository>();

    mockRepo.Setup(r => r.Delete(It.Is<int>(id => id <= 0)))
            .Throws(new ArgumentException("Invalid ID"));

    Assert.Throws<ArgumentException>(() => mockRepo.Object.Delete(-1));
}
```

### Multiple interfaces

```csharp
[Fact]
public void As_ImplementsAdditionalInterfaces()
{
    var mock = new Mock<IOrderRepository>();

    // Add IDisposable interface to the mock
    mock.As<IDisposable>()
        .Setup(d => d.Dispose())
        .Verifiable();

    // Use both interfaces
    var repo = mock.Object;
    ((IDisposable)repo).Dispose();

    mock.As<IDisposable>().Verify(d => d.Dispose(), Times.Once());
}
```

---

## AutoFixture — `CodeBrix.TestMocks.AutoFixture`

AutoFixture generates anonymous test data, removing the tedium of manually constructing test objects. It creates instances of any type with randomized but valid values.

### Basic object creation

```csharp
using CodeBrix.TestMocks.AutoFixture;
using Xunit;

public class AutoFixtureBasicTests
{
    [Fact]
    public void Create_GeneratesAnonymousValues()
    {
        var fixture = new Fixture();

        // Primitive types
        string name = fixture.Create<string>();       // e.g. "name1af4e3b0-..."
        int number = fixture.Create<int>();            // e.g. 42
        DateTime date = fixture.Create<DateTime>();    // a random DateTime
        Guid id = fixture.Create<Guid>();              // a random Guid

        Assert.False(string.IsNullOrEmpty(name));
        Assert.NotEqual(default, id);
    }

    [Fact]
    public void Create_GeneratesComplexObjects()
    {
        var fixture = new Fixture();

        // Complex objects — all properties are auto-populated
        var order = fixture.Create<Order>();

        Assert.NotEqual(0, order.Id);
        Assert.False(string.IsNullOrEmpty(order.CustomerName));
        Assert.NotEqual(0m, order.Total);
    }

    [Fact]
    public void CreateMany_GeneratesCollections()
    {
        var fixture = new Fixture();

        IEnumerable<Order> orders = fixture.CreateMany<Order>();     // default count (3)
        IEnumerable<Order> fiveOrders = fixture.CreateMany<Order>(5); // specific count

        Assert.Equal(3, orders.Count());
        Assert.Equal(5, fiveOrders.Count());
    }
}
```

### Freezing values

`Freeze` creates a value and ensures the **same instance** is used everywhere the fixture resolves that type — essential for verifying that dependencies share the same object:

```csharp
[Fact]
public void Freeze_ReturnsSameInstanceEveryTime()
{
    var fixture = new Fixture();

    // Freeze a string — every subsequent Create<string>() returns this same value
    string frozenName = fixture.Freeze<string>();

    string name1 = fixture.Create<string>();
    string name2 = fixture.Create<string>();

    Assert.Equal(frozenName, name1);
    Assert.Equal(frozenName, name2);
}

[Fact]
public void Freeze_UsefulForSharedDependencies()
{
    var fixture = new Fixture();

    // Freeze an Order so that any object created by the fixture
    // that depends on Order will receive this exact instance
    var frozenOrder = fixture.Freeze<Order>();
    frozenOrder.CustomerName = "Alice";

    // If another class takes an Order parameter, it will receive frozenOrder
    var order = fixture.Create<Order>();
    Assert.Equal("Alice", order.CustomerName);
}
```

### Customizing object creation

```csharp
[Fact]
public void Build_CustomizesSpecificProperties()
{
    var fixture = new Fixture();

    var order = fixture.Build<Order>()
                       .With(o => o.CustomerName, "SpecificCustomer")
                       .With(o => o.Total, 250.00m)
                       .Without(o => o.IsProcessed)  // leave as default
                       .Create();

    Assert.Equal("SpecificCustomer", order.CustomerName);
    Assert.Equal(250.00m, order.Total);
    Assert.False(order.IsProcessed);
}

[Fact]
public void Customize_AppliesConventionToAllInstances()
{
    var fixture = new Fixture();

    // All Orders created by this fixture will have IsProcessed = true
    fixture.Customize<Order>(c => c.With(o => o.IsProcessed, true));

    var order1 = fixture.Create<Order>();
    var order2 = fixture.Create<Order>();

    Assert.True(order1.IsProcessed);
    Assert.True(order2.IsProcessed);
}
```

---

## AutoFixture + Mocking — `CodeBrix.TestMocks.AutoFixture.AutoMock`

The `AutoMockCustomization` bridges AutoFixture with the mocking framework, so when AutoFixture encounters an **interface** or **abstract class** it cannot construct, it automatically creates a `Mock<T>` for it:

```csharp
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.AutoMock;
using CodeBrix.TestMocks.Mocking;
using Xunit;

// A service that depends on interfaces
public class OrderProcessor
{
    private readonly IOrderRepository _repository;
    private readonly IEmailService _emailService;

    public OrderProcessor(IOrderRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public void Process(int orderId)
    {
        var order = _repository.GetById(orderId);
        if (order != null)
        {
            order.IsProcessed = true;
            _repository.Save(order);
            _emailService.SendEmail(order.CustomerName, "Processed", $"Order #{order.Id} done.");
        }
    }
}

public class OrderProcessorTests
{
    [Fact]
    public void Process_SavesAndSendsEmail()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });

        // Freeze the mocks so we can set them up and verify later
        var mockRepo = fixture.Freeze<Mock<IOrderRepository>>();
        var mockEmail = fixture.Freeze<Mock<IEmailService>>();

        var order = new Order { Id = 7, CustomerName = "Alice" };
        mockRepo.Setup(r => r.GetById(7)).Returns(order);

        // AutoFixture will inject the frozen mocks into OrderProcessor's constructor
        var processor = fixture.Create<OrderProcessor>();

        // Act
        processor.Process(7);

        // Assert
        Assert.True(order.IsProcessed);
        mockRepo.Verify(r => r.Save(It.Is<Order>(o => o.Id == 7)), Times.Once());
        mockEmail.Verify(e => e.SendEmail("Alice", "Processed", It.IsAny<string>()), Times.Once());
    }
}
```

---

## xUnit v3 Integration — `CodeBrix.TestMocks.AutoFixture.Xunit3`

The xUnit v3 integration attributes let you inject auto-generated data directly into test method parameters, dramatically reducing test boilerplate.

### `[AutoData]` — fully auto-generated parameters

```csharp
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using Xunit;

public class AutoDataTests
{
    // All parameters are auto-generated by AutoFixture
    [Theory, AutoData]
    public void StringIsNeverNullOrEmpty(string value)
    {
        Assert.False(string.IsNullOrEmpty(value));
    }

    [Theory, AutoData]
    public void OrderHasPopulatedProperties(Order order)
    {
        Assert.NotEqual(0, order.Id);
        Assert.False(string.IsNullOrEmpty(order.CustomerName));
    }

    [Theory, AutoData]
    public void MultipleParameters(string name, int count, Order order)
    {
        Assert.False(string.IsNullOrEmpty(name));
        Assert.NotEqual(0, count);
        Assert.NotNull(order);
    }
}
```

### `[InlineAutoData]` — combine explicit and auto-generated data

Provide some values explicitly; AutoFixture fills in the rest:

```csharp
public class InlineAutoDataTests
{
    [Theory]
    [InlineAutoData("Alice")]
    [InlineAutoData("Bob")]
    public void Greeting_ContainsName(string name, int orderId)
    {
        // 'name' is the explicitly provided value ("Alice" or "Bob")
        // 'orderId' is auto-generated by AutoFixture
        var greeting = $"Hello {name}, your order #{orderId} is ready.";

        Assert.Contains(name, greeting);
        Assert.NotEqual(0, orderId);
    }
}
```

### `[MemberAutoData]` — combine member data with auto-generated data

```csharp
public class MemberAutoDataTests
{
    public static IEnumerable<object[]> OrderAmounts()
    {
        yield return new object[] { 10.00m };
        yield return new object[] { 99.99m };
        yield return new object[] { 250.00m };
    }

    [Theory]
    [MemberAutoData(nameof(OrderAmounts))]
    public void Order_TotalIsExpected(decimal expectedTotal, string customerName)
    {
        // 'expectedTotal' comes from OrderAmounts member data
        // 'customerName' is auto-generated by AutoFixture
        var order = new Order { Total = expectedTotal, CustomerName = customerName };

        Assert.Equal(expectedTotal, order.Total);
        Assert.False(string.IsNullOrEmpty(order.CustomerName));
    }
}
```

### `[Frozen]` — freeze a parameter for shared dependency injection

The `[Frozen]` attribute freezes a parameter's value so that the same instance is injected everywhere AutoFixture uses that type — perfect for verifying mock interactions:

```csharp
public class FrozenAttributeTests
{
    [Theory, AutoData]
    public void Frozen_EnsuresSameInstanceIsInjected(
        [Frozen] string customerName,
        Order order)
    {
        // Because customerName is [Frozen], AutoFixture will use that exact string
        // for every string property it populates — including order.CustomerName
        Assert.Equal(customerName, order.CustomerName);
    }
}
```

---

## Putting it all together — Real-world xUnit v3 test patterns

### Built-in `[AutoMockData]` and `[InlineAutoMockData]` attributes

The `CodeBrix.TestMocks.AutoFixture.AutoMock.Data` namespace provides ready-to-use xUnit v3 attributes

- **`[AutoMockData]`** — like `[AutoData]`, but the fixture is pre-configured with `AutoMockCustomization { ConfigureMembers = true }`, so interface and abstract class parameters are automatically mocked.
- **`[InlineAutoMockData]`** — like `[InlineAutoData]`, with the same auto-mocking configuration.

### Full integration test using `[AutoMockData]`

```csharp
using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
using CodeBrix.TestMocks.AutoFixture.Xunit3;
using CodeBrix.TestMocks.Mocking;
using Xunit;

public class OrderProcessorIntegrationTests
{
    [Theory, AutoMockData]
    public void Process_WhenOrderExists_ProcessesAndNotifies(
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        Order order,
        OrderProcessor sut)    // sut = "system under test", auto-constructed with frozen mocks
    {
        // Arrange
        mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);

        // Act
        sut.Process(order.Id);

        // Assert
        Assert.True(order.IsProcessed);
        mockRepo.Verify(r => r.Save(order), Times.Once());
        mockEmail.Verify(
            e => e.SendEmail(order.CustomerName, "Processed", It.IsAny<string>()),
            Times.Once());
    }

    [Theory, AutoMockData]
    public void Process_WhenOrderNotFound_DoesNotSendEmail(
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        int orderId,
        OrderProcessor sut)
    {
        // Arrange — GetById returns null (no setup needed for Loose mock)

        // Act
        sut.Process(orderId);

        // Assert
        mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Never());
        mockEmail.Verify(
            e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never());
    }

    [Theory]
    [InlineAutoMockData(42)]
    [InlineAutoMockData(99)]
    public void Process_WithSpecificOrderIds(
        int orderId,
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        OrderProcessor sut)
    {
        // Arrange
        var order = new Order { Id = orderId, CustomerName = "TestCustomer" };
        mockRepo.Setup(r => r.GetById(orderId)).Returns(order);

        // Act
        sut.Process(orderId);

        // Assert
        Assert.True(order.IsProcessed);
        mockRepo.Verify(r => r.Save(order), Times.Once());
    }
}
```

### Testing async service methods

```csharp
public class AsyncOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IEmailService _emailService;

    public AsyncOrderService(IOrderRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<Order> GetAndNotifyAsync(int orderId, string recipientEmail)
    {
        var order = await _repository.GetByIdAsync(orderId);
        if (order != null)
        {
            await _emailService.SendEmailAsync(recipientEmail, "Order Found", $"Order #{order.Id}");
        }
        return order;
    }
}

public class AsyncOrderServiceTests
{
    [Theory, AutoMockData]
    public async Task GetAndNotifyAsync_WhenOrderExists_SendsEmail(
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        Order order,
        string recipientEmail,
        AsyncOrderService sut)
    {
        // Arrange
        mockRepo.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        mockEmail.Setup(e => e.SendEmailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.GetAndNotifyAsync(order.Id, recipientEmail);

        // Assert
        Assert.Equal(order.Id, result.Id);
        mockEmail.Verify(
            e => e.SendEmailAsync(recipientEmail, "Order Found", It.IsAny<string>()),
            Times.Once());
    }
}
```

### Creating custom data attributes

The built-in `[AutoMockData]` and `[InlineAutoMockData]` attributes cover the most common case, but you can inherit from `AutoDataAttribute` or `InlineAutoDataAttribute` to apply your own fixture customizations — for example, domain-specific conventions, additional `ICustomization` implementations, or pre-configured behaviors:

```csharp
using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.AutoMock;
using CodeBrix.TestMocks.AutoFixture.Xunit3;

/// <summary>
/// A custom AutoData attribute that configures every Order
/// sets a repeat count of 5, and enables auto-mocking.
/// </summary>
public class DomainAutoDataAttribute : AutoDataAttribute
{
    public DomainAutoDataAttribute()
        : base(() =>
        {
            var fixture = new Fixture();

            // Enable auto-mocking for interfaces and abstract classes
            fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });

            // Domain convention: all auto-generated Orders are pre-processed
            fixture.Customize<Order>(c => c
                .With(o => o.IsProcessed, true)
                .With(o => o.Total, 100.00m));

            // Change the default "many" count from 3 to 5
            fixture.RepeatCount = 5;

            return fixture;
        })
    {
    }
}

public class DomainAutoDataTests
{
    [Theory, DomainAutoData]
    public void Orders_ArePreProcessedByConvention(Order order)
    {
        Assert.True(order.IsProcessed);
        Assert.Equal(100.00m, order.Total);
    }

    [Theory, DomainAutoData]
    public void CreateMany_ReturnsCustomRepeatCount(IEnumerable<Order> orders)
    {
        Assert.Equal(5, orders.Count());
    }

    [Theory, DomainAutoData]
    public void MocksAreStillAutoGenerated(
        [Frozen] Mock<IOrderRepository> mockRepo,
        OrderProcessor sut)
    {
        // The fixture auto-creates Mock<IOrderRepository> and injects it into OrderProcessor
        mockRepo.Setup(r => r.GetById(1)).Returns(new Order { Id = 1 });
        sut.Process(1);
        mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Once());
    }
}
```

You can do the same with `InlineAutoDataAttribute` to combine explicit inline values with your custom fixture configuration:

```csharp
/// <summary>
/// A custom InlineAutoData attribute with auto-mocking and domain conventions.
/// The first parameters come from inline values; the rest are auto-generated.
/// </summary>
public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
{
    public DomainInlineAutoDataAttribute(params object[] values)
        : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });
            fixture.Customize<Order>(c => c.With(o => o.IsProcessed, true));
            return fixture;
        }, values)
    {
    }
}

public class DomainInlineAutoDataTests
{
    [Theory]
    [DomainInlineAutoData("Alice", 10.00)]
    [DomainInlineAutoData("Bob", 250.00)]
    public void Process_UsesInlineCustomerAndTotal(
        string customerName,       // from inline values
        double total,              // from inline values
        [Frozen] Mock<IOrderRepository> mockRepo,  // auto-generated mock
        OrderProcessor sut)        // auto-constructed with frozen mock
    {
        var order = new Order
        {
            Id = 1,
            CustomerName = customerName,
            Total = (decimal)total
        };
        mockRepo.Setup(r => r.GetById(1)).Returns(order);

        sut.Process(1);

        mockRepo.Verify(r => r.Save(It.Is<Order>(o => o.CustomerName == customerName)), Times.Once());
    }
}
```

---

## License

CodeBrix.TestMocks is licensed under the Apache License 2.0 - see the LICENSE file in the Source Repository.

This project includes code derived from several open source projects. See the THIRD-PARTY-NOTICES.txt file in the Source Repository for complete attribution and license details.
