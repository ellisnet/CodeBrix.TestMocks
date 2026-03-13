================================================================================
AGENT-README: CodeBrix.TestMocks
A Comprehensive Guide for AI Coding Agents
================================================================================

OVERVIEW
--------
CodeBrix.TestMocks is a comprehensive, single-package .NET testing library
that provides mocking and auto-generated test data capabilities. It combines
forks of Moq (mocking) and AutoFixture (test data generation) into one unified
NuGet package with built-in xUnit v3 integration.

The library gives you everything needed to write thorough, maintainable unit
tests: mock implementations of interfaces/abstract classes, argument matchers,
verification, auto-generated anonymous test data, and xUnit v3 data-driven
test attributes.

IMPORTANT: If you are familiar with Moq and AutoFixture, the API surfaces are
essentially identical, but ALL namespaces use "CodeBrix.TestMocks" prefixes.
Do NOT use "Moq" or "AutoFixture" namespaces.

Source Repository: https://github.com/ellisnet/CodeBrix.TestMocks
License: Apache License 2.0

Incorporates code from:
  - Castle.Core / DynamicProxy (Apache 2.0)
  - Moq (BSD 3-Clause)
  - AutoFixture (MIT)
  - Fare (BSD 3-Clause + Apache 2.0)
  - TypeNameFormatter (MIT)
See THIRD-PARTY-NOTICES.txt for full attribution.

================================================================================

INSTALLATION
------------
NuGet Package: CodeBrix.TestMocks.ApacheLicenseForever
Version: 1.0.73
Authors: Jeremy Ellis
Dependencies:
  - xunit.v3.extensibility.core (>= 3.2.2)

Requirements: .NET 10.0 or higher

To add to a .NET 10+ test project:

    dotnet add package CodeBrix.TestMocks.ApacheLicenseForever

Or in a .csproj file:

    <PackageReference Include="CodeBrix.TestMocks.ApacheLicenseForever" Version="1.0.73" />

IMPORTANT: The package name is "CodeBrix.TestMocks.ApacheLicenseForever"
(not just "CodeBrix.TestMocks"). Always use this full package name.

================================================================================

KEY NAMESPACES
--------------

    using CodeBrix.TestMocks.Mocking;                // Mock<T>, It, Times, MockBehavior
    using CodeBrix.TestMocks.Mocking.Protected;       // Protected member setup/verify
    using CodeBrix.TestMocks.AutoFixture;              // Fixture, Create, Freeze
    using CodeBrix.TestMocks.AutoFixture.AutoMock;     // AutoMockCustomization
    using CodeBrix.TestMocks.AutoFixture.Xunit3;       // [AutoData], [InlineAutoData], etc.
    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data; // [AutoMockData], [InlineAutoMockData]

NOTE: The namespaces use "CodeBrix.TestMocks" prefixes (NOT "Moq" or
"AutoFixture"). This is the most common mistake when migrating from Moq or
AutoFixture.

================================================================================

MOCKING API (CodeBrix.TestMocks.Mocking)
==========================================

CREATING MOCKS
--------------

    using CodeBrix.TestMocks.Mocking;

    // Default behavior (Loose - returns defaults for unexpected calls)
    var mock = new Mock<IMyService>();

    // Strict behavior (throws on unexpected calls)
    var mock = new Mock<IMyService>(MockBehavior.Strict);

    // With constructor arguments (for mocking classes)
    var mock = new Mock<MyClass>(arg1, arg2);

    // Get the mocked instance
    IMyService service = mock.Object;

SETUP AND RETURNS
-----------------

    // Return a specific value
    mock.Setup(s => s.GetById(42)).Returns(expectedItem);

    // Return based on argument
    mock.Setup(s => s.GetById(It.IsAny<int>()))
        .Returns<int>(id => new Item { Id = id });

    // Computed return value
    mock.Setup(s => s.GetName()).Returns(() => "computed");

    // Void method setup
    mock.Setup(s => s.Save(It.IsAny<Item>()));

    // Throw exception
    mock.Setup(s => s.GetById(-1)).Throws<ArgumentException>();
    mock.Setup(s => s.GetById(-1)).Throws(new ArgumentException("Invalid"));

    // Call base implementation (for class mocks)
    mock.Setup(s => s.VirtualMethod()).CallBase();

ARGUMENT MATCHERS (It class)
-----------------------------

    It.IsAny<T>()                                    // Matches any value
    It.IsNotNull<T>()                                // Matches non-null
    It.Is<T>(x => x > 0)                             // Matches predicate
    It.IsInRange(1, 100, Range.Inclusive)             // Range match
    It.IsIn(new[] { 1, 2, 3 })                       // In collection
    It.IsNotIn(new[] { 4, 5, 6 })                    // Not in collection
    It.IsRegex(@"\d+")                                // Regex match
    It.Is<T>(value, comparer)                         // Custom comparer

Type matchers for generic methods:

    It.IsAnyType                                      // Matches any generic type
    It.IsSubtype<T>                                   // Matches subtypes of T
    It.IsValueType                                    // Matches any value type

Ref parameter matcher:

    It.Ref<T>.IsAny                                   // Matches any ref param

ASYNC MOCKING
--------------

    // Task<T> returning methods
    mock.Setup(s => s.GetByIdAsync(42))
        .ReturnsAsync(expectedItem);

    // Task returning void methods
    mock.Setup(s => s.SaveAsync(It.IsAny<Item>()))
        .Returns(Task.CompletedTask);

    // Async exception
    mock.Setup(s => s.SaveAsync(It.IsAny<Item>()))
        .ThrowsAsync(new InvalidOperationException("DB error"));

CALLBACKS
----------

    // Execute code when method is called
    var captured = new List<Item>();
    mock.Setup(s => s.Save(It.IsAny<Item>()))
        .Callback<Item>(item => captured.Add(item));

    // Callback with return
    mock.Setup(s => s.GetById(It.IsAny<int>()))
        .Callback<int>(id => Console.WriteLine($"Getting {id}"))
        .Returns<int>(id => new Item { Id = id });

VERIFICATION
-------------

    // Verify method was called
    mock.Verify(s => s.Save(It.IsAny<Item>()));

    // Verify with specific Times
    mock.Verify(s => s.Save(It.IsAny<Item>()), Times.Once());
    mock.Verify(s => s.Delete(It.IsAny<int>()), Times.Never());
    mock.Verify(s => s.GetById(42), Times.Exactly(2));
    mock.Verify(s => s.GetById(It.IsAny<int>()), Times.AtLeastOnce());
    mock.Verify(s => s.GetById(It.IsAny<int>()), Times.AtMost(3));

    // Verify with custom message
    mock.Verify(s => s.Save(It.Is<Item>(i => i.Id == 1)),
                Times.Once(), "Expected item 1 to be saved");

    // Verify no other unexpected calls
    mock.VerifyNoOtherCalls();

    // Verify all setups marked as Verifiable()
    mock.Verify();

    // Verify ALL setups (including non-verifiable ones)
    mock.VerifyAll();

Times struct:

    Times.Once()
    Times.Never()
    Times.AtLeastOnce()
    Times.AtLeast(n)
    Times.AtMostOnce()
    Times.AtMost(n)
    Times.Exactly(n)
    Times.Between(from, to, Range.Inclusive)

PROPERTY MOCKING
-----------------

    // Track property get/set
    mock.SetupProperty(s => s.Name);
    mock.SetupProperty(s => s.Name, "initial value");

    // Track ALL properties
    mock.SetupAllProperties();

    // Setup getter
    mock.SetupGet(s => s.Name).Returns("fixed value");

    // Setup setter
    mock.SetupSet(s => s.Name = "expected");

SEQUENCES
----------

    mock.SetupSequence(s => s.GetById(It.IsAny<int>()))
        .Returns(new Item { Id = 1 })    // First call
        .Returns(new Item { Id = 2 })    // Second call
        .Throws<InvalidOperationException>();  // Third call throws

MULTIPLE INTERFACES
--------------------

    var mock = new Mock<IOrderRepository>();
    mock.As<IDisposable>()
        .Setup(d => d.Dispose())
        .Verifiable();

CONDITIONAL SETUP
------------------

    mock.When(() => someCondition)
        .Setup(s => s.GetById(It.IsAny<int>()))
        .Returns(specialItem);

MOCK SEQUENCES (ordered verification)
---------------------------------------

    var sequence = new MockSequence();
    mock1.InSequence(sequence).Setup(s => s.First());
    mock2.InSequence(sequence).Setup(s => s.Second());

    // Cyclic sequence
    sequence.Cyclic = true;

MOCK REPOSITORY
----------------

    var repo = new MockRepository(MockBehavior.Strict);
    var mock1 = repo.Create<IService1>();
    var mock2 = repo.Create<IService2>();
    // ...
    repo.Verify();     // Verify all mocks
    repo.VerifyAll();  // Verify all setups on all mocks

PROTECTED MEMBERS
------------------

    using CodeBrix.TestMocks.Mocking.Protected;

    mock.Protected()
        .Setup("ProtectedMethod", ItExpr.IsAny<int>())
        .Returns("result");

    mock.Protected()
        .Verify("ProtectedMethod", Times.Once(), ItExpr.IsAny<int>());

    // Type-safe analog approach
    mock.Protected().As<IAnalog>()
        .Setup(a => a.Method(It.IsAny<int>()))
        .Returns("result");

EVENT RAISING
--------------

    mock.Raise(s => s.MyEvent += null, EventArgs.Empty);
    mock.Raise(s => s.MyEvent += null, arg1, arg2);
    await mock.RaiseAsync(s => s.MyEvent += null, arg1);

ARGUMENT CAPTURE
-----------------

    using CodeBrix.TestMocks.Mocking;

    var captured = new List<Item>();
    mock.Setup(s => s.Save(Capture.In(captured)));

    // Or with predicate
    mock.Setup(s => s.Save(Capture.In(captured, i => i.Id > 0)));

STATIC HELPERS
---------------

    // Get Mock<T> from a mocked instance
    Mock<IService> mock = Mock.Get(mockedInstance);

    // Reset mock state
    mock.Reset();

    // LINQ-style mock creation
    var service = Mock.Of<IService>(s => s.Name == "Test" && s.Id == 42);

================================================================================

AUTOFIXTURE API (CodeBrix.TestMocks.AutoFixture)
==================================================

CREATING TEST DATA
-------------------

    using CodeBrix.TestMocks.AutoFixture;

    var fixture = new Fixture();

    // Create anonymous instances
    string name = fixture.Create<string>();       // e.g., "name1af4e3b0-..."
    int number = fixture.Create<int>();            // Random int
    var order = fixture.Create<Order>();           // Auto-populated object
    DateTime date = fixture.Create<DateTime>();

    // Create multiple
    IEnumerable<Order> orders = fixture.CreateMany<Order>();       // Default: 3
    IEnumerable<Order> fiveOrders = fixture.CreateMany<Order>(5);

FREEZING (shared instances)
----------------------------

    var fixture = new Fixture();
    string frozenName = fixture.Freeze<string>();
    // All subsequent Create<string>() calls return frozenName

    var frozenRepo = fixture.Freeze<Mock<IRepository>>();
    // All subsequent requests for IRepository or Mock<IRepository> return same

CUSTOMIZING
------------

    // Builder pattern for specific instances
    var order = fixture.Build<Order>()
        .With(o => o.CustomerName, "SpecificCustomer")
        .With(o => o.Total, 250.00m)
        .Without(o => o.IsProcessed)    // Skip this property
        .Create();

    // Register customization for all instances of a type
    fixture.Customize<Order>(c => c
        .With(o => o.IsProcessed, true)
        .Without(o => o.Id));

INJECTION
----------

    // Inject a specific instance for a type
    fixture.Inject<ILogger>(myLogger);

    // Register a factory
    fixture.Register<ILogger>(() => new TestLogger());

    // Register with dependencies
    fixture.Register<string, ILogger>(name => new TestLogger(name));

FIXTURE PROPERTIES
-------------------

    fixture.RepeatCount = 5;             // Default count for CreateMany
    fixture.OmitAutoProperties = true;   // Don't auto-populate properties

================================================================================

AUTOMOCK INTEGRATION (CodeBrix.TestMocks.AutoFixture.AutoMock)
===============================================================

Combines AutoFixture with mocking so that constructor dependencies are
automatically resolved with mocks.

    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.AutoMock;
    using CodeBrix.TestMocks.Mocking;

    var fixture = new Fixture();
    fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });

    // Freeze a mock to control it
    var mockRepo = fixture.Freeze<Mock<IOrderRepository>>();

    // Create the SUT - constructor dependencies auto-injected with mocks
    var processor = fixture.Create<OrderProcessor>();

    // Setup behavior
    mockRepo.Setup(r => r.GetById(It.IsAny<int>()))
            .Returns(new Order { Id = 42 });

    // Act
    processor.Process(42);

    // Verify
    mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Once());

AutoMockCustomization properties:

    ConfigureMembers = true              // Auto-setup mock members
    GenerateDelegates = true             // Create delegates via Mock

================================================================================

XUNIT V3 ATTRIBUTES (CodeBrix.TestMocks.AutoFixture.Xunit3)
=============================================================

[AutoData] - auto-generate all test parameters:

    using CodeBrix.TestMocks.AutoFixture.Xunit3;

    [Theory, AutoData]
    public void MyTest(string name, int id, Order order)
    {
        // name, id, and order are auto-generated
    }

[InlineAutoData] - mix inline and auto-generated:

    [Theory]
    [InlineAutoData("Alice")]
    [InlineAutoData("Bob")]
    public void MyTest(string name, int autoGeneratedId)
    {
        // name is from InlineAutoData, autoGeneratedId is auto-generated
    }

[MemberAutoData] - member data + auto-generated:

    public static IEnumerable<object[]> OrderAmounts =>
        new[] { new object[] { 10m }, new object[] { 20m } };

    [Theory]
    [MemberAutoData(nameof(OrderAmounts))]
    public void MyTest(decimal amount, string autoGeneratedName)
    {
        // amount from member data, name auto-generated
    }

[Frozen] - freeze a parameter across the test:

    [Theory, AutoData]
    public void MyTest([Frozen] string customerName, Order order)
    {
        // customerName is frozen, so order.CustomerName == customerName
        Assert.Equal(customerName, order.CustomerName);
    }

    // Matching modes (what to freeze against)
    [Frozen(Matching.ExactType)]
    [Frozen(Matching.ImplementedInterfaces)]
    [Frozen(Matching.ParameterName)]
    [Frozen(Matching.MemberName)]

================================================================================

AUTOMOCK DATA ATTRIBUTES (CodeBrix.TestMocks.AutoFixture.AutoMock.Data)
========================================================================

[AutoMockData] - auto-generate with mocking support:

    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
    using CodeBrix.TestMocks.Mocking;

    [Theory, AutoMockData]
    public void MyTest(
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        Order order,
        OrderProcessor sut)
    {
        // mockRepo and mockEmail are frozen mocks
        // order is auto-generated
        // sut (OrderProcessor) has its constructor dependencies auto-injected

        mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);
        sut.Process(order.Id);
        mockRepo.Verify(r => r.Save(order), Times.Once());
    }

[InlineAutoMockData] - inline values + auto-mock:

    [Theory]
    [InlineAutoMockData(42)]
    [InlineAutoMockData(99)]
    public void MyTest(
        int orderId,
        [Frozen] Mock<IOrderRepository> mockRepo,
        OrderProcessor sut)
    {
        // orderId is from inline data, rest is auto-generated with mocking
    }

CUSTOM DATA ATTRIBUTES
-----------------------

Create reusable, pre-configured test data attributes:

    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute()
            : base(() =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoMockCustomization
                {
                    ConfigureMembers = true
                });
                fixture.Customize<Order>(c => c.With(o => o.IsProcessed, true));
                fixture.RepeatCount = 5;
                return fixture;
            }) { }
    }

    public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DomainInlineAutoDataAttribute(params object[] values)
            : base(() =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoMockCustomization
                {
                    ConfigureMembers = true
                });
                return fixture;
            }, values) { }
    }

    // Usage:
    [Theory, DomainAutoData]
    public void MyTest(Order order, OrderProcessor sut) { ... }

    [Theory]
    [DomainInlineAutoData(42)]
    public void MyTest(int id, Order order, OrderProcessor sut) { ... }

================================================================================

OTHER XUNIT3 ATTRIBUTES
========================

    [ClassAutoData(typeof(MyDataClass))]   // Class-sourced auto data
    [FavorArrays]                          // Prefer T[] over IList<T>
    [FavorEnumerables]                     // Prefer IEnumerable<T>
    [FavorLists]                           // Prefer List<T>
    [Greedy]                               // Use greediest constructor
    [Modest]                               // Use most modest constructor
    [NoAutoProperties]                     // Skip auto-populating properties

Matching enum (for [Frozen]):

    Matching.ExactType = 1
    Matching.DirectBaseType = 2
    Matching.ImplementedInterfaces = 4
    Matching.ParameterName = 8
    Matching.PropertyName = 16
    Matching.FieldName = 32
    Matching.MemberName = ParameterName | PropertyName | FieldName

================================================================================

FLUENT SETUP API CHAIN
========================

    mock.Setup(x => x.Method(args))
        .Returns(value)                     // Return specific value
        .Returns(Func<TResult>)             // Return computed value
        .Returns<T1>(Func<T1, TResult>)     // Return based on argument
        .ReturnsAsync(value)                // For Task<T>-returning methods
        .ReturnsAsync(Func<TResult>)
        .Throws(Exception)                  // Throw exception
        .Throws<TException>()
        .ThrowsAsync(Exception)             // For Task-returning methods
        .Callback(Action)                   // Execute callback on invocation
        .Callback<T1>(Action<T1>)           // Callback with argument access
        .CallBase()                         // Call base implementation
        .Verifiable()                       // Mark for Verify()
        .Verifiable(string failMessage)

    mock.SetupSequence(x => x.Method(args))
        .Returns(value)                     // First call
        .Returns(value2)                    // Second call
        .Throws(exception)                  // Third call throws

    mock.SetupProperty(x => x.Prop)         // Enable get/set tracking
    mock.SetupProperty(x => x.Prop, val)    // With initial value
    mock.SetupAllProperties()               // Stub all properties

================================================================================

COMPLETE EXAMPLES
=================

Example 1: Basic Mocking
--------------------------
    using CodeBrix.TestMocks.Mocking;

    public interface IOrderRepository
    {
        Order GetById(int id);
        void Save(Order order);
        void Delete(int id);
    }

    [Fact]
    public void ProcessOrder_SavesOrder()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order { Id = 42, CustomerName = "Alice", Total = 99.95m };
        mockRepo.Setup(r => r.GetById(42)).Returns(order);

        var processor = new OrderProcessor(mockRepo.Object);

        // Act
        processor.Process(42);

        // Assert
        mockRepo.Verify(r => r.Save(It.Is<Order>(o => o.Id == 42)), Times.Once());
        mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never());
    }

Example 2: AutoFixture + AutoMock
------------------------------------
    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.AutoMock;
    using CodeBrix.TestMocks.Mocking;

    [Fact]
    public void ProcessOrder_WithAutoFixture()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });

        var mockRepo = fixture.Freeze<Mock<IOrderRepository>>();
        var order = fixture.Create<Order>();
        mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);

        var processor = fixture.Create<OrderProcessor>();

        // Act
        processor.Process(order.Id);

        // Assert
        mockRepo.Verify(r => r.Save(order), Times.Once());
    }

Example 3: Data-Driven Test with [AutoMockData]
--------------------------------------------------
    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
    using CodeBrix.TestMocks.AutoFixture.Xunit3;
    using CodeBrix.TestMocks.Mocking;

    [Theory, AutoMockData]
    public void ProcessOrder_DataDriven(
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        Order order,
        OrderProcessor sut)
    {
        // Arrange
        mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);

        // Act
        sut.Process(order.Id);

        // Assert
        mockRepo.Verify(r => r.Save(order), Times.Once());
        mockEmail.Verify(e => e.SendConfirmation(order.CustomerName), Times.Once());
    }

Example 4: Async Mocking
---------------------------
    using CodeBrix.TestMocks.Mocking;

    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task SaveAsync(Order order);
    }

    [Fact]
    public async Task ProcessOrderAsync_SavesOrder()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(42))
                .ReturnsAsync(new Order { Id = 42 });

        var processor = new OrderProcessor(mockRepo.Object);
        await processor.ProcessAsync(42);

        mockRepo.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Once());
    }

================================================================================

COMMON USING STATEMENT COMBINATIONS
=====================================

For basic mocking:

    using CodeBrix.TestMocks.Mocking;

For mocking with AutoFixture:

    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.AutoMock;
    using CodeBrix.TestMocks.Mocking;

For xUnit v3 data-driven tests with mocking:

    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
    using CodeBrix.TestMocks.AutoFixture.Xunit3;
    using CodeBrix.TestMocks.Mocking;

For protected member testing:

    using CodeBrix.TestMocks.Mocking;
    using CodeBrix.TestMocks.Mocking.Protected;

================================================================================

WHAT THIS LIBRARY DOES NOT DO
===============================

Do NOT attempt to use CodeBrix.TestMocks for the following - it will not work:

  - Mocking sealed classes or non-virtual members of concrete classes
    (only interfaces, abstract classes, and virtual/abstract members)
  - Mocking static methods or static classes
  - Providing its own test runner (it relies on xUnit v3)
  - Supporting xUnit v2 or earlier (only xUnit v3)
  - Providing assertion methods (use xUnit Assert, SilverAssertions, etc.)
  - Integration/end-to-end testing
  - HTTP mocking, database faking, or file system abstractions
  - NSubstitute or FakeItEasy APIs (it uses the Moq API pattern)
  - Generating realistic/production-quality test data (e.g., real names);
    it generates anonymous values like "name1af4e3b0-..." for strings
  - Running on .NET versions below 10.0

This library IS for: creating mock objects, setting up behavior, verifying
interactions, generating anonymous test data, and integrating all of this
with xUnit v3 data-driven tests, all from a single NuGet package.

================================================================================

MINIMUM VIABLE PROJECT TEMPLATE
=================================

To scaffold a new .NET 10 test project that uses CodeBrix.TestMocks:

    dotnet new xunit -n MyTests --framework net10.0
    cd MyTests
    dotnet add package CodeBrix.TestMocks.ApacheLicenseForever

Then in a test file:

    using CodeBrix.TestMocks.Mocking;

    public interface IGreeter { string Greet(string name); }

    public class GreeterTests
    {
        [Fact]
        public void Greet_ReturnsExpectedMessage()
        {
            var mock = new Mock<IGreeter>();
            mock.Setup(g => g.Greet("Alice")).Returns("Hello, Alice!");

            Assert.Equal("Hello, Alice!", mock.Object.Greet("Alice"));
            mock.Verify(g => g.Greet("Alice"), Times.Once());
        }
    }

Build and run:

    dotnet build
    dotnet test

================================================================================

PERFORMANCE TIPS FOR CODING AGENTS
====================================

1. USE [AutoMockData]: This single attribute gives you auto-generated data
   AND auto-mocking. It's the most productive way to write tests.

2. USE [Frozen]: Freeze mocks to share instances between the SUT's constructor
   injection and your test setup. Without [Frozen], each parameter gets a
   different mock instance.

3. USE Loose BEHAVIOR (default): Only use MockBehavior.Strict when you
   specifically need to verify that NO unexpected calls are made. Strict
   mocks cause brittle tests.

4. USE SPECIFIC MATCHERS: Prefer It.Is<T>(x => x.Id == 42) over
   It.IsAny<T>() for more precise verification.

5. USE SetupAllProperties(): When you need to track property changes across
   many properties, this is cleaner than individual SetupProperty calls.

6. USE Fixture.Freeze<Mock<T>>(): This is the standard pattern for getting
   a controllable mock that's also injected into the SUT via AutoMock.

7. USE CreateMany<T>(count): When you need collections of test data, use
   CreateMany instead of manual loops.

8. PREFER [AutoMockData] OVER MANUAL FIXTURE: The attribute handles fixture
   creation, customization, and parameter injection automatically.

================================================================================

COMMON PITFALLS TO AVOID
=========================

1. DO NOT confuse the NuGet package name with the namespace.
   - Package: CodeBrix.TestMocks.ApacheLicenseForever
   - Namespaces: CodeBrix.TestMocks.Mocking, CodeBrix.TestMocks.AutoFixture, etc.

2. DO NOT use "Moq" or "AutoFixture" namespaces. They are
   CodeBrix.TestMocks.Mocking and CodeBrix.TestMocks.AutoFixture.

3. DO NOT forget [Frozen] when you need a mock injected into the SUT.
   Without it, the mock parameter and the SUT's dependency are different
   instances.

4. DO NOT use MockBehavior.Strict unless necessary. It makes tests brittle
   by throwing on any call you didn't explicitly set up.

5. DO NOT target .NET versions below 10.0. This library requires .NET 10+.

6. DO NOT try to mock sealed classes, non-virtual methods, or static members.
   Only interfaces, abstract classes, and virtual/abstract members work.

7. DO NOT forget that AutoFixture generates ANONYMOUS data, not realistic
   data. Strings look like "nameb4a3f..." not "John Smith".

8. DO NOT use xUnit v2 attributes. This library only supports xUnit v3.
   Use [Theory] from xunit.v3, not from xunit.

9. DO NOT forget to call mock.Object to get the mocked instance from Mock<T>.
   Pass mock.Object (not mock) to the code under test.

================================================================================

DEEPER LEARNING: TEST FILE CROSS-REFERENCES
=============================================

The CodeBrix.TestMocks source repository contains extensive test files.
If the documentation above is not sufficient, read the relevant files from
the local repository at: /home/debian/GitHome/CodeBrix.TestMocks

Test project: tests/CodeBrix.TestMocks.Tests/

Feature-to-test-file mapping:

  Mock.As<TInterface>() (casting mocks to additional interfaces):
    -> tests/CodeBrix.TestMocks.Tests/AsInterfaceTests.cs

  Times struct (validation of call counts):
    -> tests/CodeBrix.TestMocks.Tests/TimesTests.cs

  .Verifiable() behavior:
    -> tests/CodeBrix.TestMocks.Tests/VerifiableSetupTests.cs

  Async awaitable support:
    -> tests/CodeBrix.TestMocks.Tests/Async/AwaitableTests.cs

  LINQ-to-Mocks:
    -> tests/CodeBrix.TestMocks.Tests/Linq/MockRepositoryQueryingTests.cs
    -> tests/CodeBrix.TestMocks.Tests/Linq/SupportedQueryingTests.cs
    -> tests/CodeBrix.TestMocks.Tests/Linq/QueryableMocksTests.cs

  AutoMockCustomization:
    -> tests/CodeBrix.TestMocks.Tests/AutoFixture/AutoMock/AutoMockCustomizationTest.cs
    -> tests/CodeBrix.TestMocks.Tests/AutoFixture/AutoMock/AutoConfiguredMockCustomizationTest.cs

  Fixture + AutoMock integration:
    -> tests/CodeBrix.TestMocks.Tests/AutoFixture/AutoMock/FixtureIntegrationWithAutoMockCustomizationTest.cs
    -> tests/CodeBrix.TestMocks.Tests/AutoFixture/AutoMock/FixtureIntegrationWithAutoConfiguredMockCustomizationTest.cs

  Expression matching:
    -> tests/CodeBrix.TestMocks.Tests/MatchExpressionTests.cs

  Callback delegate validation:
    -> tests/CodeBrix.TestMocks.Tests/AfterReturnCallbackDelegateValidationTests.cs

  Property hiding/new keyword:
    -> tests/CodeBrix.TestMocks.Tests/HidePropertyTests.cs

  ActionObserver behavior:
    -> tests/CodeBrix.TestMocks.Tests/ActionObserverTests.cs

NOTE: Since this repository is not yet public, fetch files from the local
copy at /home/debian/GitHome/CodeBrix.TestMocks/ instead of GitHub URLs.

================================================================================

QUICK REFERENCE CARD
=====================

Install:         dotnet add package CodeBrix.TestMocks.ApacheLicenseForever
Mock NS:         using CodeBrix.TestMocks.Mocking;
AutoFixture NS:  using CodeBrix.TestMocks.AutoFixture;
AutoMock NS:     using CodeBrix.TestMocks.AutoFixture.AutoMock;
xUnit3 NS:       using CodeBrix.TestMocks.AutoFixture.Xunit3;
AutoMockData NS: using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;

Create mock:     new Mock<IService>()
Get instance:    mock.Object
Setup:           mock.Setup(s => s.Method(args)).Returns(value)
Setup async:     mock.Setup(s => s.MethodAsync(args)).ReturnsAsync(value)
Verify:          mock.Verify(s => s.Method(args), Times.Once())
Any matcher:     It.IsAny<T>()
Predicate:       It.Is<T>(x => condition)
Strict:          new Mock<IService>(MockBehavior.Strict)
Sequence:        mock.SetupSequence(s => s.Method()).Returns(v1).Returns(v2)
Properties:      mock.SetupAllProperties()
Protected:       mock.Protected().Setup("MethodName", args)

Create data:     fixture.Create<T>()
Create many:     fixture.CreateMany<T>()
Freeze:          fixture.Freeze<T>()
Build custom:    fixture.Build<T>().With(x => x.Prop, val).Create()
Auto mock:       fixture.Customize(new AutoMockCustomization())

[Theory, AutoData]           // All params auto-generated
[Theory, AutoMockData]       // Auto-generated with mocking
[InlineAutoData("val")]      // Mix inline + auto
[InlineAutoMockData("val")]  // Mix inline + auto-mock
[Frozen]                     // Freeze parameter
[MemberAutoData(nameof(M))]  // Member data + auto

Target:          .NET 10.0+
License:         Apache 2.0

================================================================================
