================================================================================
AGENT-README: CodeBrix.TestMocks
A Guide for AI Coding Agents — CONSUMING the
CodeBrix.TestMocks.ApacheLicenseForever NuGet package
================================================================================

OVERVIEW
========

CodeBrix.TestMocks is a single-package .NET testing library that provides
mocking, dynamic proxy generation and auto-generated test data. It targets
.NET 10 or later.

One package gives you everything needed to write thorough unit tests: mock
implementations of interfaces and abstract classes, argument matchers, call
verification, event simulation, auto-generated anonymous test data, an
extensible object-graph generation kernel, auto-mocking of constructor
dependencies, and xUnit v3 data-driven test attributes.

PROVENANCE. The library consolidates the source of several upstream open
source projects under CodeBrix.TestMocks.* namespaces: Moq (mocking) becomes
CodeBrix.TestMocks.Mocking; AutoFixture (test data) becomes
CodeBrix.TestMocks.AutoFixture; AutoFixture's auto-mocking glue becomes
CodeBrix.TestMocks.AutoFixture.AutoMock; Castle DynamicProxy becomes
CodeBrix.TestMocks.DynamicProxy (with its logging abstraction under
CodeBrix.TestMocks.Logging); Fare (regex-driven string generation) becomes
CodeBrix.TestMocks.Fare. The public API shapes are essentially those of the
upstream projects.

DO NOT use the upstream namespaces (Moq, AutoFixture, AutoFixture.Xunit3,
Castle.DynamicProxy, Fare). They do not exist in this package. Every namespace
starts with CodeBrix.TestMocks. This is the single most common mistake when
porting existing test code onto this package.

Source repository: https://github.com/ellisnet/CodeBrix.TestMocks

================================================================================

INSTALLATION
============

PackageId: CodeBrix.TestMocks.ApacheLicenseForever

    dotnet add package CodeBrix.TestMocks.ApacheLicenseForever

Or in a .csproj:

    <PackageReference Include="CodeBrix.TestMocks.ApacheLicenseForever" />

IMPORTANT: the package id is CodeBrix.TestMocks.ApacheLicenseForever — NOT
"CodeBrix.TestMocks". The assembly and the namespace root are
CodeBrix.TestMocks; only the package id carries the ApacheLicenseForever
suffix.

NuGet dependencies:
  - xunit.v3.extensibility.core   (needed for the xUnit v3 data attributes)

License: Apache-2.0. The nupkg requires license acceptance. Third-party
attribution for the consolidated upstream sources ships in the package as
THIRD-PARTY-NOTICES.txt (Castle.Core / DynamicProxy — Apache-2.0; Moq — BSD
3-Clause; AutoFixture — MIT; Fare — BSD 3-Clause and Apache-2.0;
TypeNameFormatter — MIT).

Requirements:
  - .NET 10 or later. There are no other target frameworks.
  - Pure managed code; no native libraries, no OS restrictions.
  - Mocking and proxy generation use runtime IL emission, so the test host must
    allow dynamic code generation (this rules out full NativeAOT / trimmed
    "no dynamic code" test hosts).

To actually RUN tests you also need the xUnit v3 test framework and a runner
in your test project — this package brings in only the extensibility core:

    <PackageReference Include="xunit.v3" Version="..." />
    <PackageReference Include="xunit.runner.visualstudio" Version="...">
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="..." />

This package provides no assertion library. Use xUnit's Assert, or a fluent
assertion package such as SilverAssertions.

================================================================================

KEY NAMESPACES / USINGS
=======================

    using CodeBrix.TestMocks.Mocking;
        Mock, Mock<T>, It, Times, Range, MockBehavior, MockRepository,
        MockSequence, MockException, DefaultValue, DefaultValueProvider,
        LookupOrFallbackDefaultValueProvider, Capture, CaptureMatch<T>, Match,
        Match<T>, IInvocation, IInvocationList, ISetup, ISetupList,
        InvocationAction, InvocationFunc, ITypeMatcher, TypeMatcherAttribute,
        Switches, ReturnsExtensions, SequenceExtensions, MockExtensions

    using CodeBrix.TestMocks.Mocking.Language;
    using CodeBrix.TestMocks.Mocking.Language.Flow;
        The fluent setup interfaces (ISetup<T>, IReturns<,>, ICallback, ...).
        Rarely imported directly; needed when you write helper methods that
        take or return a setup mid-chain.

    using CodeBrix.TestMocks.Mocking.Protected;
        ProtectedExtension (mock.Protected()), IProtectedMock<T>,
        IProtectedAsMock<T, TAnalog>, ItExpr

    using CodeBrix.TestMocks.AutoFixture;
        Fixture, IFixture, ICustomization, CompositeCustomization,
        ObjectCreationException, the Create/CreateMany/Build/Freeze/Inject/
        Register/AddManyTo/Repeat extension methods, the built-in
        customizations, behaviors and primitive generators

    using CodeBrix.TestMocks.AutoFixture.Kernel;
        ISpecimenBuilder, ISpecimenContext, SpecimenContext, NoSpecimen,
        OmitSpecimen, IRequestSpecification, ISpecimenCommand,
        ISpecimenBuilderNode, ISpecimenBuilderTransformation, the request
        types, relays, specifications and constructor queries

    using CodeBrix.TestMocks.AutoFixture.Dsl;
        ICustomizationComposer<T>, IPostprocessComposer<T>, IFactoryComposer<T>
        (the return types of fixture.Build<T>() and its chain)

    using CodeBrix.TestMocks.AutoFixture.DataAnnotations;
        NoDataAnnotationsCustomization and the data-annotation relays

    using CodeBrix.TestMocks.AutoFixture.AutoMock;
        AutoMockCustomization, AutoConfiguredMockCustomization, MockRelay,
        MockType.ReturnsUsingFixture

    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
        [AutoMockData], [InlineAutoMockData]

    using CodeBrix.TestMocks.AutoFixture.Xunit3;
        [AutoData], [InlineAutoData], [MemberAutoData], [ClassAutoData],
        [Frozen], Matching, [Greedy], [Modest], [FavorArrays],
        [FavorEnumerables], [FavorLists], [NoAutoProperties],
        CustomizeAttribute, CompositeDataAttribute

    using CodeBrix.TestMocks.DynamicProxy;
        ProxyGenerator, IInterceptor, IInvocation, ProxyGenerationOptions,
        IProxyGenerationHook, IInterceptorSelector, ProxyUtil,
        StandardInterceptor, AllMethodsHook

    using CodeBrix.TestMocks.Fare;
        Xeger, RegExp, Automaton (regex-driven string generation)

    using CodeBrix.TestMocks.Logging;
        ILogger, IExtendedLogger, LoggerLevel, NullLogger, TraceLogger
        (the logging abstraction used by the proxy generator)

NOTE: CodeBrix.TestMocks.Mocking.IInvocation and
CodeBrix.TestMocks.DynamicProxy.IInvocation are two different types. If you
import both namespaces in one file you must disambiguate.

================================================================================

CORE API REFERENCE — MOCKING (CodeBrix.TestMocks.Mocking)
=========================================================

Creating mocks
--------------

    Mock<T>()                                   // MockBehavior.Loose (default)
    Mock<T>(MockBehavior behavior)
    Mock<T>(params object[] args)               // ctor args for class mocks
    Mock<T>(MockBehavior behavior, params object[] args)
    Mock<T>(Expression<Func<T>> newExpression,
            MockBehavior behavior = MockBehavior.Default)

    var mock = new Mock<IMyService>();
    var strict = new Mock<IMyService>(MockBehavior.Strict);
    var withCtor = new Mock<MyClass>(arg1, arg2);
    var byNewExpr = new Mock<MyClass>(() => new MyClass(arg1, arg2));

    IMyService service = mock.Object;   // the instance to hand to the SUT

MockBehavior: Strict, Loose, Default (Default == Loose).
Loose returns default values for calls you did not set up; Strict throws a
MockException on any call that has no matching setup.

Instance members on Mock<T> / Mock:

    T        Object              { get; }
    string   Name                { get; set; }   // used in exception messages
    MockBehavior Behavior        { get; }
    bool     CallBase            { get; set; }
    DefaultValue DefaultValue    { get; set; }
    DefaultValueProvider DefaultValueProvider { get; set; }
    Switches Switches            { get; set; }
    IInvocationList Invocations  { get; }
    ISetupList Setups            { get; }
    Mock<TInterface> As<TInterface>() where TInterface : class
    void     SetReturnsDefault<TReturn>(TReturn value)
    void     Verify()            // only setups marked Verifiable()
    void     VerifyAll()         // every setup
    void     VerifyNoOtherCalls()

Static members on Mock:

    static Mock<T> Get<T>(T mocked) where T : class
    static void Verify(params Mock[] mocks)
    static void VerifyAll(params Mock[] mocks)
    static T Of<T>() where T : class
    static T Of<T>(MockBehavior behavior)
    static T Of<T>(Expression<Func<T, bool>> predicate)
    static T Of<T>(Expression<Func<T, bool>> predicate, MockBehavior behavior)

Extension methods:

    mock.Reset()                 // MockExtensions.Reset(this Mock) — clears
                                 // setups, configured default return values,
                                 // registered event handlers and all
                                 // recorded invocations

Setup and returns
-----------------

    ISetup<T>             Setup(Expression<Action<T>> expression)
    ISetup<T, TResult>    Setup<TResult>(Expression<Func<T, TResult>> expression)
    ISetupGetter<T, TProperty> SetupGet<TProperty>(
                              Expression<Func<T, TProperty>> expression)
    ISetupSetter<T, TProperty> SetupSet<TProperty>(Action<T> setterExpression)
    ISetup<T>             SetupSet(Action<T> setterExpression)
    ISetup<T>             SetupAdd(Action<T> addExpression)
    ISetup<T>             SetupRemove(Action<T> removeExpression)
    Mock<T>               SetupProperty<TProperty>(
                              Expression<Func<T, TProperty>> property)
    Mock<T>               SetupProperty<TProperty>(
                              Expression<Func<T, TProperty>> property,
                              TProperty initialValue)
    Mock<T>               SetupAllProperties()
    ISetupSequentialResult<TResult> SetupSequence<TResult>(
                              Expression<Func<T, TResult>> expression)
    ISetupSequentialAction SetupSequence(Expression<Action<T>> expression)
    ISetupConditionResult<T> When(Func<bool> condition)

Examples:

    mock.Setup(s => s.GetById(42)).Returns(expectedItem);
    mock.Setup(s => s.GetById(It.IsAny<int>()))
        .Returns<int>(id => new Item { Id = id });
    mock.Setup(s => s.GetName()).Returns(() => "computed");
    mock.Setup(s => s.Save(It.IsAny<Item>()));            // void method
    mock.Setup(s => s.GetById(-1)).Throws<ArgumentException>();
    mock.Setup(s => s.GetById(-1)).Throws(new ArgumentException("Invalid"));
    mock.Setup(s => s.VirtualMethod()).CallBase();        // class mocks only

Setup expressions may chain through properties ("recursive mocks"): the
intermediate mocks are created for you.

    mock.SetupGet(m => m.Bar.Value).Returns(5);
    int five = mock.Object.Bar.Value;
    Mock<IBar> barMock = Mock.Get(mock.Object.Bar);   // reachable afterwards

Argument matchers (It)
----------------------

    It.IsAny<TValue>()
    It.IsNotNull<TValue>()
    It.Is<TValue>(Expression<Func<TValue, bool>> match)
    It.Is<TValue>(Expression<Func<object, Type, bool>> match)
    It.Is<TValue>(TValue value, IEqualityComparer<TValue> comparer)
    It.IsInRange<TValue>(TValue from, TValue to, Range rangeKind)
    It.IsIn<TValue>(IEnumerable<TValue> items)
    It.IsIn<TValue>(IEnumerable<TValue> items, IEqualityComparer<TValue>)
    It.IsIn<TValue>(params TValue[] items)
    It.IsNotIn<TValue>(IEnumerable<TValue> items)
    It.IsNotIn<TValue>(IEnumerable<TValue> items, IEqualityComparer<TValue>)
    It.IsNotIn<TValue>(params TValue[] items)
    It.IsRegex(string regex)
    It.IsRegex(string regex, RegexOptions options)

Range is CodeBrix.TestMocks.Mocking.Range { Inclusive, Exclusive }. If your
file also uses System.Range, alias it:
    using Range = CodeBrix.TestMocks.Mocking.Range;

Type matchers (for setting up generic methods without naming the type
argument) — used AS the type argument, not as a value:

    It.IsAnyType                 // sealed class, ITypeMatcher: any type
    It.IsSubtype<T>              // sealed class, ITypeMatcher: T or a subtype
    It.IsValueType               // readonly struct, ITypeMatcher: value types

    mock.Setup(x => x.Method<It.IsAnyType>()).Callback(() => count++);
    mock.Verify(m => m.Method<It.IsSubtype<ArgumentException>>(),
                Times.Exactly(3));
    mock.Setup(m => m.Method<It.IsAnyType>((It.IsAnyType)It.IsAny<object>()));

A custom type matcher is a type implementing ITypeMatcher with a public
parameterless constructor:

    public sealed class IntOrString : ITypeMatcher
    {
        public bool Matches(Type typeArgument)
            => typeArgument == typeof(int) || typeArgument == typeof(string);
    }

    mock.Setup(x => x.Method<IntOrString>()).Callback(() => count++);
    mock.Verify(x => x.Method<IntOrString>(), Times.Exactly(3));

TypeMatcherAttribute(Type type) marks a type as standing in for a type matcher
when the matcher itself cannot be used directly (for example because it has no
parameterless constructor).

Ref/out parameters:

    It.Ref<TValue>.IsAny         // static field, matches any ref/out argument

    string expected = "ack";
    mock.Setup(m => m.Execute("ping", out expected)).Returns(true);
    mock.Setup(m => m.Do(ref It.Ref<int>.IsAny)).Callback(() => called++);
    mock.Verify(m => m.Do(ref It.Ref<int>.IsAny), Times.Exactly(2));

Custom value matchers (Match)
-----------------------------

Match.Create builds a reusable matcher that can be called inside a setup or
verify expression like It.IsAny:

    static class Match  // CodeBrix.TestMocks.Mocking.Match
    {
        static T Create<T>(Predicate<T> condition);
        static T Create<T>(Predicate<T> condition,
                           Expression<Func<T>> renderExpression);
        static T Create<T>(Func<object, Type, bool> condition,
                           Expression<Func<T>> renderExpression);
    }

    public static TValue Positive<TValue>() where TValue : IComparable
        => Match.Create<TValue>(v => v != null && v.CompareTo(default) > 0);

    mock.Setup(m => m.Do(Positive<int>())).Returns(true);

The renderExpression overloads control how the matcher is printed in failure
messages. Match<T> is the concrete matcher class; CaptureMatch<T> derives from
it (see "Argument capture"). MatcherAttribute exists but is obsolete — use
Match.Create instead.

Async mocking
-------------

Extension methods in ReturnsExtensions and GeneratedReturnsExtensions, applied
to IReturns<TMock, Task<TResult>> and IReturns<TMock, ValueTask<TResult>>:

    ReturnsAsync(TResult value)
    ReturnsAsync(Func<TResult> valueFunction)
    ReturnsAsync<T1..T16>(Func<T1..T16, TResult> valueFunction)
    ReturnsAsync(TResult value, TimeSpan delay)
    ReturnsAsync(TResult value, TimeSpan minDelay, TimeSpan maxDelay)
    ReturnsAsync(TResult value, TimeSpan minDelay, TimeSpan maxDelay,
                 Random random)
    ThrowsAsync(Exception exception)                 // Task and ValueTask too
    ThrowsAsync(Exception exception, TimeSpan delay)
    ThrowsAsync(Exception exception, TimeSpan minDelay, TimeSpan maxDelay)

    mock.Setup(s => s.GetByIdAsync(42)).ReturnsAsync(expectedItem);
    mock.Setup(s => s.SaveAsync(It.IsAny<Item>())).Returns(Task.CompletedTask);
    mock.Setup(s => s.SaveAsync(It.IsAny<Item>()))
        .ThrowsAsync(new InvalidOperationException("DB error"));
    mock.Setup(s => s.LoadAsync()).ReturnsAsync(item, TimeSpan.FromSeconds(1));

For sequences, SequenceExtensions adds ReturnsAsync / ThrowsAsync / PassAsync
to ISetupSequentialResult<Task>, <Task<T>>, <ValueTask> and <ValueTask<T>>:

    mock.SetupSequence(s => s.GetAsync())
        .ReturnsAsync(first)
        .ReturnsAsync(second)
        .ThrowsAsync(new TimeoutException());

Callbacks
---------

    ICallbackResult Callback(Action action)
    ICallbackResult Callback<T1..T16>(Action<T1..T16> action)
    ICallbackResult Callback(Delegate callback)
    ICallbackResult Callback(InvocationAction action)

    var captured = new List<Item>();
    mock.Setup(s => s.Save(It.IsAny<Item>()))
        .Callback<Item>(item => captured.Add(item));

    mock.Setup(s => s.GetById(It.IsAny<int>()))
        .Callback<int>(id => log.Add(id))
        .Returns<int>(id => new Item { Id = id });

InvocationAction / InvocationFunc give the whole invocation, which is the way
to observe generic type arguments or write to ref/out parameters:

    mock.Setup(m => m.Method(It.IsAny<int>()))
        .Callback(new InvocationAction(inv =>
            Console.WriteLine(inv.Method.Name + ": " + inv.Arguments[0])));

    mock.Setup(m => m.Compute(It.IsAny<int>()))
        .Returns(new InvocationFunc(inv => (int)inv.Arguments[0] * 2));

Verification
------------

    mock.Verify(s => s.Save(It.IsAny<Item>()));
    mock.Verify(s => s.Save(It.IsAny<Item>()), Times.Once());
    mock.Verify(s => s.Delete(It.IsAny<int>()), Times.Never());
    mock.Verify(s => s.GetById(42), Times.Exactly(2));
    mock.Verify(s => s.Save(It.Is<Item>(i => i.Id == 1)),
                Times.Once(), "Expected item 1 to be saved");
    mock.VerifyGet(s => s.Name, Times.Once());
    mock.VerifySet(s => s.Name = "expected", Times.Once());
    mock.VerifyAdd(s => s.Added += It.IsAny<EventHandler>(), Times.Exactly(2));
    mock.VerifyRemove(s => s.Added -= It.IsAny<EventHandler>(), Times.Once());
    mock.VerifyNoOtherCalls();
    mock.Verify();      // only setups marked .Verifiable()
    mock.VerifyAll();   // all setups

Every Verify/VerifyGet/VerifySet/VerifyAdd/VerifyRemove overload accepts, in
addition to the expression: nothing, Times, Func<Times>, string failMessage,
(Times, string) or (Func<Times>, string).

Times (readonly struct):

    Times.Once()      Times.Never()          Times.AtLeastOnce()
    Times.AtLeast(n)  Times.AtMostOnce()     Times.AtMost(n)
    Times.Exactly(n)  Times.Between(from, to, Range rangeKind)
    bool Validate(int count)
    void Deconstruct(out int from, out int to)

Verifiable marks a setup for the parameterless Verify():

    void Verifiable()
    void Verifiable(string failMessage)
    void Verifiable(Times times)
    void Verifiable(Func<Times> times)
    void Verifiable(Times times, string failMessage)
    void Verifiable(Func<Times> times, string failMessage)

Failed verification (and unexpected calls on strict mocks) throws
MockException, which exposes bool IsVerificationError.

Inspecting what happened (Invocations and Setups)
-------------------------------------------------

    interface IInvocationList : IReadOnlyList<IInvocation> { void Clear(); }

    interface IInvocation
    {
        MethodInfo Method { get; }
        IReadOnlyList<object> Arguments { get; }
        ISetup MatchingSetup { get; }
        bool IsVerified { get; }
        object ReturnValue { get; }
        Exception Exception { get; }
    }

    interface ISetupList : IReadOnlyList<ISetup> { }

    interface ISetup
    {
        LambdaExpression Expression { get; }
        Mock Mock { get; }
        Mock InnerMock { get; }
        Expression OriginalExpression { get; }
        bool IsConditional { get; }
        bool IsMatched { get; }
        bool IsOverridden { get; }
        bool IsVerifiable { get; }
        void Verify(bool recursive = true);
        void VerifyAll();
    }

    mock.Object.CompareTo(other);
    Assert.Single(mock.Invocations);
    var inv = mock.Invocations[0];
    Assert.Equal(typeof(IComparable).GetMethod("CompareTo"), inv.Method);
    Assert.Equal(new[] { other }, inv.Arguments);
    mock.Invocations.Clear();      // forget recorded calls, keep the setups

This is the escape hatch when an assertion is easier to express over the
recorded calls than as a Verify expression.

Default values for unmatched calls
----------------------------------

    enum DefaultValue { Empty, Mock, Custom }

    mock.DefaultValue = DefaultValue.Empty;   // default: 0, null, empty array
    mock.DefaultValue = DefaultValue.Mock;    // mockable returns become mocks

    var mock = new Mock<IFoo> { DefaultValue = DefaultValue.Mock };
    IBar bar = mock.Object.Bar;      // auto-created mock, same instance twice

Assigning DefaultValue.Custom throws; "Custom" is the value the property
REPORTS when a custom provider is installed. To install one, set
DefaultValueProvider:

    abstract class DefaultValueProvider
    {
        static DefaultValueProvider Empty { get; }
        static DefaultValueProvider Mock  { get; }
        protected internal abstract object GetDefaultValue(Type type, Mock mock);
        protected internal virtual object GetDefaultParameterValue(
            ParameterInfo parameter, Mock mock);
        protected internal virtual object GetDefaultReturnValue(
            MethodInfo method, Mock mock);
    }

The practical base class is LookupOrFallbackDefaultValueProvider, which lets
you register a factory per type (or per open generic type) and falls back to
the default strategy for everything else:

    public sealed class MyValues : LookupOrFallbackDefaultValueProvider
    {
        public MyValues()
        {
            Register(typeof(string), (type, mock) => "(none)");
            Register(typeof(IEnumerable<>),
                     (type, mock) => Array.CreateInstance(
                         type.GetGenericArguments()[0], 0));
        }
    }

    mock.DefaultValueProvider = new MyValues();
    // protected members available to subclasses:
    //   void Register(Type factoryKey, Func<Type, Mock, object> factory)
    //   void Deregister(Type factoryKey)
    //   virtual object GetFallbackDefaultValue(Type type, Mock mock)

For a single return type, SetReturnsDefault is simpler:

    mock.SetReturnsDefault<bool>(true);   // every unmatched bool-returning
                                          // member now returns true

CallBase
--------

Mock<T>.CallBase is a mock-wide switch: when true, calls that have no matching
setup invoke the real (base) implementation instead of returning a default.
The same idea per setup is the CallBase() step in the fluent chain, and
ISetupSequentialResult<T>.CallBase() for one item of a sequence.

    var mock = new Mock<MyClass> { CallBase = true };
    mock.Setup(m => m.OneMethod()).Returns(42);   // everything else is real

CallBase only applies to class mocks (virtual/abstract members).

Properties
----------

    mock.SetupProperty(s => s.Name);              // read/write stub
    mock.SetupProperty(s => s.Name, "initial");
    mock.SetupAllProperties();                    // stub every property
    mock.SetupGet(s => s.Name).Returns("fixed");
    mock.SetupSet(s => s.Name = "expected");
    mock.SetupSet<string>(s => s.Name = "expected").Callback(v => seen = v);

Events
------

Subscription and unsubscription are ordinary invocations, so they can be set
up, verified and given callbacks:

    mock.SetupAdd(m => m.Added += It.IsAny<EventHandler>());
    mock.SetupRemove(m => m.Added -= It.IsAny<EventHandler>());
    mock.SetupAdd(m => m.Added += It.IsAny<EventHandler>())
        .Callback(() => subscribed = true);
    mock.SetupAdd(m => m.VirtualEvent += It.IsAny<EventHandler>()).CallBase();

    mock.VerifyAdd(m => m.Added += It.IsAny<EventHandler>(), Times.Exactly(2));
    mock.VerifyRemove(m => m.Added -= It.IsAny<EventHandler>(), Times.Once());

Raising an event from the mock:

    void Raise(Action<T> eventExpression, EventArgs args)
    void Raise(Action<T> eventExpression, params object[] args)
    Task RaiseAsync(Action<T> eventExpression, params object[] args)

    mock.Raise(s => s.MyEvent += null, EventArgs.Empty);
    mock.Raise(s => s.MyEvent += null, sender, new MyEventArgs(7));
    await mock.RaiseAsync(s => s.MyEvent += null, EventArgs.Empty);

Raising an event as a side effect of a call — the IRaise<T> step of the chain:

    IVerifies Raises(Action<T> eventExpression, EventArgs args)
    IVerifies Raises(Action<T> eventExpression, Func<EventArgs> func)
    IVerifies Raises(Action<T> eventExpression, params object[] args)
    IVerifies Raises<T1..T16>(Action<T> eventExpression,
                              Func<T1..T16, EventArgs> func)

    mock.Setup(s => s.Save(It.IsAny<Item>()))
        .Raises(s => s.Saved += null, EventArgs.Empty);

Sequences
---------

    ISetupSequentialResult<TResult>:
        Returns(TResult value)
        Returns(Func<TResult> valueFunction)
        Throws(Exception exception)
        Throws<TException>() where TException : Exception, new()
        Throws<TException>(Func<TException> exceptionFunction)
        CallBase()

    ISetupSequentialAction:
        Pass()
        Throws(Exception exception)
        Throws<TException>()
        Throws<TException>(Func<TException> exceptionFunction)

    mock.SetupSequence(s => s.GetById(It.IsAny<int>()))
        .Returns(new Item { Id = 1 })
        .Returns(new Item { Id = 2 })
        .Throws<InvalidOperationException>();

    mock.SetupSequence(s => s.Save(It.IsAny<Item>()))
        .Pass()
        .Throws<IOException>();

Multiple interfaces, conditional setups and ordering
----------------------------------------------------

    mock.As<IDisposable>().Setup(d => d.Dispose()).Verifiable();

    mock.When(() => isReady)                       // ISetupConditionResult<T>
        .Setup(s => s.GetById(It.IsAny<int>()))
        .Returns(specialItem);

ISetupConditionResult<T> exposes Setup, Setup<TResult>, SetupGet, SetupSet and
SetupSet<TProperty>.

    var sequence = new MockSequence { Cyclic = false };
    mock1.InSequence(sequence).Setup(s => s.First());
    mock2.InSequence(sequence).Setup(s => s.Second());

InSequence is the MockSequenceHelper extension method on Mock<TMock>; it
returns ISetupConditionResult<TMock>. Set Cyclic = true to let the sequence
restart after its last step.

MockRepository
--------------

    var repo = new MockRepository(MockBehavior.Strict);
    repo.DefaultValue = DefaultValue.Mock;    // applied to created mocks
    repo.CallBase = true;
    var mock1 = repo.Create<IService1>();
    var mock2 = repo.Create<IService2>(MockBehavior.Loose);
    var mock3 = repo.Create<MyClass>(ctorArg1, ctorArg2);
    ...
    repo.Verify();              // Verifiable setups on every created mock
    repo.VerifyAll();           // every setup on every created mock
    repo.VerifyNoOtherCalls();

Create overloads: Create<T>(), Create<T>(params object[] args),
Create<T>(MockBehavior), Create<T>(MockBehavior, params object[] args),
Create<T>(Expression<Func<T>> newExpression, MockBehavior behavior =
MockBehavior.Default).

Protected members
-----------------

    using CodeBrix.TestMocks.Mocking.Protected;

    mock.Protected()                       // IProtectedMock<T>
        .Setup<string>("ProtectedMethod", ItExpr.IsAny<int>())
        .Returns("result");

    mock.Protected()
        .Verify("ProtectedMethod", Times.Once(), ItExpr.IsAny<int>());

    mock.Protected().SetupGet<int>("Count").Returns(3);
    mock.Protected().SetupSequence<int>("Next").Returns(1).Returns(2);

IProtectedMock<T> members take a member name and, optionally, a
bool exactParameterMatch and/or a Type[] genericTypeArguments:
Setup, Setup<TResult>, SetupGet<TProperty>, SetupSet<TProperty>,
SetupSequence, SetupSequence<TResult>, Verify, Verify<TResult>,
VerifyGet<TProperty>, VerifySet<TProperty>, As<TAnalog>().

The type-safe alternative declares an "analog" interface that mirrors the
protected members, and uses ordinary lambda expressions and It matchers:

    public interface IMyClassProtected { string Compute(int value); }

    mock.Protected().As<IMyClassProtected>()      // IProtectedAsMock<T, TAnalog>
        .Setup(a => a.Compute(It.IsAny<int>()))
        .Returns("result");

ItExpr is the matcher set for the string-based API (it returns Expression,
not a value): ItExpr.IsAny<T>(), ItExpr.IsNull<T>(), ItExpr.Is<T>(expr),
ItExpr.IsInRange<T>(from, to, Range), ItExpr.IsRegex(pattern[, options]),
ItExpr.Ref<T>.IsAny.

Argument capture
----------------

    var captured = new List<Item>();
    mock.Setup(s => s.Save(Capture.In(captured)));
    mock.Setup(s => s.Save(Capture.In(captured, i => i.Id > 0)));
    mock.Setup(s => s.Save(Capture.With(new CaptureMatch<Item>(
        i => captured.Add(i), i => i.Id > 0))));

    static T Capture.In<T>(ICollection<T> collection)
    static T Capture.In<T>(IList<T> collection, Expression<Func<T, bool>> pred)
    static T Capture.With<T>(CaptureMatch<T> match)

LINQ to Mocks
-------------

    var service = Mock.Of<IService>(s => s.Name == "Test" && s.Id == 42);
    var strictService = Mock.Of<IService>(s => s.Id == 1, MockBehavior.Strict);

    IQueryable<IService> many = Mocks.Of<IService>();
    IQueryable<IService> filtered = Mocks.Of<IService>(s => s.Id > 0);
    IService one = Mocks.OneOf<IService>(s => s.Name == "Test");

Mocks (CodeBrix.TestMocks.Mocking.Linq) exposes Of<T>(), Of<T>(MockBehavior),
Of<T>(specification), Of<T>(specification, MockBehavior), OneOf<T>() and
OneOf<T>(specification). MockRepository has the same six as instance methods
so the mocks are tracked for repository-wide verification.

The fluent chain and its interface types
----------------------------------------

Knowing these names matters when you write a helper that returns a
half-configured setup. All live in CodeBrix.TestMocks.Mocking.Language and
CodeBrix.TestMocks.Mocking.Language.Flow.

    Setup(void method)      -> ISetup<TMock>
                               : ICallback, ICallbackResult, IRaise<TMock>,
                                 IVerifies
    Setup<TResult>(...)     -> ISetup<TMock, TResult>
                               : ICallback<TMock, TResult>,
                                 IReturnsThrows<TMock, TResult>, IVerifies
    SetupGet<TProperty>     -> ISetupGetter<TMock, TProperty>
    SetupSet<TProperty>     -> ISetupSetter<TMock, TProperty>
    When(...)               -> ISetupConditionResult<T>
    SetupSequence           -> ISetupSequentialResult<TResult>
                               / ISetupSequentialAction

    .Callback(...)          -> ICallbackResult            (void setups)
                            -> IReturnsThrows<TMock,TResult> (value setups)
    .Returns(...)           -> IReturnsResult<TMock>
    .Throws(...)            -> IThrowsResult
    .CallBase()             -> ICallBaseResult
    .Raises(...)            -> IVerifies
    .Verifiable(...)        -> void

    IReturns<TMock, TResult>   Returns(TResult), Returns(Func<TResult>),
                               Returns<T1..T16>(Func<...,TResult>),
                               Returns(Delegate), Returns(InvocationFunc),
                               CallBase()
    IThrows                    Throws(Exception), Throws<TException>(),
                               Throws(Delegate),
                               Throws<TException>(Func<TException>),
                               Throws<T, TException>(Func<T, TException>)
    ICallBase                  CallBase()
    IVerifies                  Verifiable() and its five overloads
    IRaise<T>                  Raises(...) overloads
    IFluentInterface           hides Equals/GetHashCode/GetType/ToString from
                               IntelliSense on all of the above

Property setups have their own three: ICallbackGetter<TMock, TProperty>,
IReturnsGetter<TMock, TProperty> and IReturnsThrowsGetter<TMock, TProperty>
for SetupGet, and ICallbackSetter<TProperty> for SetupSet.

Two more interfaces appear on generated mock objects rather than on Mock<T>:
IMocked (Mock Mock { get; }) and IMocked<T> (new Mock<T> Mock { get; }) are
what Mock.Get uses to find the Mock behind an instance, and IMock<out T>
(Object, Behavior, CallBase, DefaultValue) is the read-only face of a mock.
ExpressionCompiler.Instance is the replaceable strategy used to compile setup
expressions, and IProxy / InterfaceProxy
(namespace CodeBrix.TestMocks.Mocking.Internals) are the seam between the
mocking API and the proxy generator.

Obsolete but still public, for source compatibility only: MockFactory (renamed
to MockRepository), MatcherAttribute (use Match.Create), IOccurrence
(AtMostOnce/AtMost on a setup — use Times), Mock<T>.Expect / ExpectGet /
ExpectSet (renamed to Setup / SetupGet / SetupSet), ObsoleteMockExtensions and
MockLegacyExtensions (older SetupSet/VerifySet shapes taking an
Expression<Func<T, TProperty>>), and ResetCalls (use
mock.Invocations.Clear()).

Diagnostics
-----------

    mock.Switches = Switches.CollectDiagnosticFileInfoForSetups;

Switches is a [Flags] enum with Default = 0 and
CollectDiagnosticFileInfoForSetups = 1. Turning it on makes verification
failure messages include the file and line of the setup.

================================================================================

CORE API REFERENCE — TEST DATA (CodeBrix.TestMocks.AutoFixture)
===============================================================

The Fixture
-----------

    public interface IFixture : ISpecimenBuilder
    {
        IList<ISpecimenBuilderTransformation> Behaviors { get; }
        IList<ISpecimenBuilder> Customizations { get; }
        IList<ISpecimenBuilder> ResidueCollectors { get; }
        bool OmitAutoProperties { get; set; }
        int RepeatCount { get; set; }
        ICustomizationComposer<T> Build<T>();
        IFixture Customize(ICustomization customization);
        void Customize<T>(
            Func<ICustomizationComposer<T>, ISpecimenBuilder>
                composerTransformation);
    }

    public class Fixture : IFixture, IEnumerable<ISpecimenBuilder>
    {
        Fixture();
        Fixture(DefaultRelays engineParts);
        Fixture(ISpecimenBuilder engine, MultipleRelay multiple);
        ISpecimenBuilder Engine { get; }
        object Create(object request, ISpecimenContext context);
    }

RepeatCount defaults to 3 (this is how many items CreateMany produces and how
many elements auto-generated collections get). OmitAutoProperties defaults to
false, so writable public properties of a created object are filled in.

Creating values
---------------

Extension methods (SpecimenFactory), available on IFixture, ISpecimenBuilder,
ISpecimenContext and IPostprocessComposer<T>:

    T Create<T>()
    IEnumerable<T> CreateMany<T>()
    IEnumerable<T> CreateMany<T>(int count)

    var fixture = new Fixture();
    string name  = fixture.Create<string>();     // "<guid>" or "<seed><guid>"
    int number   = fixture.Create<int>();
    var order    = fixture.Create<Order>();      // ctor args + auto properties
    DateTime dt  = fixture.Create<DateTime>();
    var three    = fixture.CreateMany<Order>();          // RepeatCount items
    var five     = fixture.CreateMany<Order>(5);

Strings are a GUID rendered as text; when the request carries a seed (for
example a property or parameter named "Name"), the seed is prefixed, giving
values like "Namef3c1...". They are anonymous values, not realistic data.

These extension methods are declared on static classes you will see in stack
traces and IntelliSense: SpecimenFactory (Create/CreateMany), FixtureFreezer
(Freeze), FixtureRegistrar (Inject/Register), FixtureRepeater (Repeat),
CollectionFiller (AddMany/AddManyTo) and CustomizationExtensions
(ToCustomization).

Other value helpers:

    void AddManyTo<T>(this IFixture, ICollection<T> collection)
    void AddManyTo<T>(this IFixture, ICollection<T> collection, int repeatCount)
    void AddManyTo<T>(this IFixture, ICollection<T> collection, Func<T> creator)
    void AddMany<T>(this ICollection<T>, Func<T> creator, int repeatCount)
    IEnumerable<T> Repeat<T>(this IFixture, Func<T> function)
    new Generator<T>(ISpecimenBuilder builder)   // infinite IEnumerable<T>

Shaping a single type (the Dsl)
-------------------------------

fixture.Build<T>() returns ICustomizationComposer<T>, which is
IFactoryComposer<T> + IPostprocessComposer<T>. The chain members:

    IFactoryComposer<T>:
        IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        IPostprocessComposer<T> FromFactory(Func<T> factory)
        IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        IPostprocessComposer<T> FromFactory<TInput1, TInput2>(...)
        IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(...)
        IPostprocessComposer<T> FromFactory<TInput1..TInput4>(...)
        IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)

    IPostprocessComposer<T>:
        IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker,
            Func<TProperty> valueFactory)
        IPostprocessComposer<T> With<TProperty, TInput>(
            Expression<Func<T, TProperty>> propertyPicker,
            Func<TInput, TProperty> valueFactory)
        IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker,
            ISpecimenBuilder builder)
        IPostprocessComposer<T> Without<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        IPostprocessComposer<T> Do(Action<T> action)
        IPostprocessComposer<T> WithAutoProperties()
        IPostprocessComposer<T> OmitAutoProperties()

Create() / CreateMany() are the extension methods that end the chain. The
concrete implementations behind these interfaces are NodeComposer<T> (the one
a Fixture returns), CompositeNodeComposer<T>, CompositePostprocessComposer<T>
and NullComposer<T> (a no-op composer).

    var order = fixture.Build<Order>()
        .With(o => o.CustomerName, "SpecificCustomer")
        .With(o => o.Total, 250.00m)
        .With(o => o.Reference, () => Guid.NewGuid().ToString("N"))
        .Without(o => o.IsProcessed)
        .Do(o => o.Lines.Add(new Line()))
        .Create();

Build<T>() is a ONE-OFF: it does not change what fixture.Create<T>() returns.
To make the shaping permanent for the fixture use Customize<T>:

    fixture.Customize<Order>(c => c
        .With(o => o.IsProcessed, true)
        .Without(o => o.Id));

Freezing and registering
------------------------

    T Freeze<T>(this IFixture fixture)
    T Freeze<T>(this IFixture fixture,
                Func<ICustomizationComposer<T>, ISpecimenBuilder>
                    composerTransformation)
    void Inject<T>(this IFixture fixture, T item)
    void Register<T>(this IFixture fixture, Func<T> creator)
    void Register<TInput, T>(this IFixture fixture, Func<TInput, T> creator)
    void Register<TInput1, TInput2, T>(...)          // up to four inputs

    string frozenName = fixture.Freeze<string>();
    // every later Create<string>() returns frozenName

    var frozenRepo = fixture.Freeze<Mock<IRepository>>();
    // later requests for Mock<IRepository> — and, with AutoMock installed,
    // for IRepository itself — resolve to that same mock

    fixture.Inject<ILogger>(myLogger);          // fixed instance
    fixture.Register<ILogger>(() => new TestLogger());        // factory
    fixture.Register<string, ILogger>(name => new TestLogger(name));

Freeze is Inject plus "create it first": it creates one specimen and injects
it. Inject/Register do not create anything up front.

Data annotations are honoured
-----------------------------

A default Fixture has data-annotation support wired into its builder graph, so
attributes on the member being populated constrain the generated value:

    [Range(1, 10)]                  -> value within the range (numeric, enum,
                                       TimeSpan, DateTime operands)
    [StringLength(20)]              -> string no longer than 20
    [MinLength(2)] / [MaxLength(8)] -> constrained string or sequence length
    [RegularExpression(@"^\d{3}$")] -> string matching the pattern
    [EnumDataType(typeof(Suit))]    -> a value of that enum

The relays that implement this live in
CodeBrix.TestMocks.AutoFixture.DataAnnotations: RangeAttributeRelay,
NumericRangedRequestRelay, EnumRangedRequestRelay, TimeSpanRangedRequestRelay,
StringLengthAttributeRelay, MinAndMaxLengthAttributeRelay,
RegularExpressionAttributeRelay and EnumDataTypeAttributeRelay, grouped by
DataAnnotationsSupportNode. They translate the attribute into a kernel request
(RangedRequest, ConstrainedStringRequest, RegularExpressionRequest) which the
primitive generators then satisfy.

To turn the whole feature off (it costs reflection time):

    fixture.Customize(new NoDataAnnotationsCustomization());

Regular-expression values are produced with the bundled Fare engine, so
[RegularExpression] patterns really do generate matching strings.

Built-in customizations (ICustomization implementations)
--------------------------------------------------------

    AutoMockCustomization                 auto-mock constructor dependencies
    AutoConfiguredMockCustomization       obsolete; use AutoMockCustomization
                                          { ConfigureMembers = true }
    CompositeCustomization                combine several into one
    ConstructorCustomization(Type, IMethodQuery)
                                          pick which constructor to use
    CurrentDateTimeCustomization          DateTime.Now for DateTime requests
    IncrementingDateTimeCustomization     each DateTime one day later
    DisposableTrackingCustomization       tracks IDisposable specimens; its
                                          Behavior property exposes the
                                          DisposableTrackingBehavior, and the
                                          customization itself is IDisposable
    FreezingCustomization(Type[, Type])   freeze a type (non-generic form)
    FreezeOnMatchCustomization(object request[, IRequestSpecification matcher])
                                          what [Frozen(Matching...)] uses
    MultipleCustomization                 CreateMany semantics for sequences
    MapCreateManyToEnumerable             map CreateMany<T>() to IEnumerable<T>
    NoAutoPropertiesCustomization(Type)   stop filling properties for a type
    NoDataAnnotationsCustomization        remove data-annotation support
    NumericSequencePerTypeCustomization   independent number sequence per type
    RandomNumericSequenceCustomization    random (not sequential) numbers
    RandomBooleanSequenceCustomization    random booleans
    RandomRangedNumberCustomization       random values inside ranges
    StableFiniteSequenceCustomization     stable finite sequences
    SupportMutableValueTypesCustomization allow mutable structs

    fixture.Customize(new CompositeCustomization(
        new AutoMockCustomization { ConfigureMembers = true },
        new CurrentDateTimeCustomization()));

Write your own by implementing ICustomization:

    public interface ICustomization { void Customize(IFixture fixture); }

Any ISpecimenBuilder can be turned into a customization:

    ICustomization c = myBuilder.ToCustomization();   // CustomizationExtensions

Behaviors (the outermost layer)
-------------------------------

fixture.Behaviors holds ISpecimenBuilderTransformation instances that wrap the
whole builder graph:

    public interface ISpecimenBuilderTransformation
    {
        ISpecimenBuilderNode Transform(ISpecimenBuilder builder);
    }

    ThrowingRecursionBehavior          throw on a circular object graph
                                       (PRESENT BY DEFAULT on new Fixture())
    OmitOnRecursionBehavior()          leave the recursive member unset
    OmitOnRecursionBehavior(int recursionDepth)
    NullRecursionBehavior()            set the recursive member to null
    NullRecursionBehavior(int recursionDepth)
    TracingBehavior()                  write the request trace to Console
    TracingBehavior(TextWriter writer)
    DisposableTrackingBehavior
    ReadonlyCollectionPropertiesBehavior([IPropertyQuery])
                                       populate get-only collection properties

Swapping the recursion behavior is the standard fix for a circular graph:

    fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => fixture.Behaviors.Remove(b));
    fixture.Behaviors.Add(new OmitOnRecursionBehavior());

The kernel (CodeBrix.TestMocks.AutoFixture.Kernel)
--------------------------------------------------

Everything is built on two one-method interfaces:

    public interface ISpecimenBuilder
    {
        object Create(object request, ISpecimenContext context);
    }

    public interface ISpecimenContext
    {
        object Resolve(object request);
    }

    public class SpecimenContext : ISpecimenContext
    {
        SpecimenContext(ISpecimenBuilder builder);
        ISpecimenBuilder Builder { get; }
        object Resolve(object request);
    }

A "request" is usually a Type, a PropertyInfo, a FieldInfo, a ParameterInfo,
or one of the kernel's own request objects. A builder that cannot handle the
request MUST return a sentinel rather than null:

    NoSpecimen.Instance      "not mine — ask someone else" (sealed class;
                             the public constructor is obsolete, use Instance)
    new OmitSpecimen()       "produce nothing at all here" (used to leave a
                             property or parameter unassigned)

Kernel request types you will meet:

    SeededRequest(object request, object seed)     .Request, .Seed
    MultipleRequest(object request)                .Request
    FiniteSequenceRequest(object request, int count)
    RangedNumberRequest(Type operandType, object minimum, object maximum)
    RangedRequest(Type memberType, Type operandType, object min, object max)
    ConstrainedStringRequest(int minimumLength, int maximumLength)
    ConstrainedStringRequest(int maximumLength)
    RegularExpressionRequest(string pattern)        .Pattern

Composition and filtering:

    CompositeSpecimenBuilder(params ISpecimenBuilder[] builders)
    FilteringSpecimenBuilder(ISpecimenBuilder builder,
                             IRequestSpecification specification)
    Postprocessor(ISpecimenBuilder builder, Action<object, ISpecimenContext>)
    Postprocessor(ISpecimenBuilder builder, ISpecimenCommand command)
    Postprocessor<T>(ISpecimenBuilder builder, Action<T, ISpecimenContext>)
    FixedBuilder(object specimen)          always returns that instance
    TypeRelay(Type from, Type to)          redirect abstraction to concrete
    ElementsBuilder<T>(params T[] elements) pick from a fixed set
    SpecimenBuilderNodeFactory, SpecimenBuilderNode      graph plumbing

Request specifications (IRequestSpecification: bool IsSatisfiedBy(object)):

    ExactTypeSpecification(Type)          DirectBaseTypeSpecification(Type)
    ImplementedInterfaceSpecification(Type)  AbstractTypeSpecification
    ParameterSpecification(Type, string)  PropertySpecification(Type, string)
    FieldSpecification(Type, string)      SeedRequestSpecification(Type)
    EqualRequestSpecification(object)     DelegateSpecification
    AndRequestSpecification(...)          OrRequestSpecification(...)
    InverseRequestSpecification(...)      TrueRequestSpecification
    FalseRequestSpecification             AnyTypeSpecification
    ValueTypeSpecification                CollectionSpecification
    ListSpecification / HashSetSpecification / DictionarySpecification /
    SortedSetSpecification / SortedListSpecification /
    SortedDictionarySpecification / ObservableCollectionSpecification
    NullableEnumRequestSpecification      NoConstructorsSpecification

Constructor and method selection
(IMethodQuery: IEnumerable<IMethod> SelectMethods(Type)):

    ModestConstructorQuery                fewest parameters (the default)
    GreedyConstructorQuery                most parameters
    ArrayFavoringConstructorQuery         EnumerableFavoringConstructorQuery
    ListFavoringConstructorQuery          FactoryMethodQuery
    InstanceMethodQuery                   CompositeMethodQuery
    TemplateMethodQuery

Property queries (IPropertyQuery, used by
ReadonlyCollectionPropertiesBehavior): AndPropertyQuery,
GenericCollectionPropertyQuery, ReadonlyPropertyQuery.

IMethod implementations (IEnumerable<ParameterInfo> Parameters;
object Invoke(IEnumerable<object> parameters)): ConstructorMethod,
InstanceMethod, StaticMethod, GenericMethod, MissingParametersSupplyingMethod.
MethodInvoker(IMethodQuery query) is the ISpecimenBuilder that puts a query to
work.

Relays (fallbacks that translate one request into another):

    SeedIgnoringRelay        ParameterRequestRelay     PropertyRequestRelay
    FieldRequestRelay        EnumerableRelay           ListRelay
    CollectionRelay          DictionaryRelay           ArrayRelay
    ReadOnlyCollectionRelay  AsyncEnumerableRelay      EnumeratorRelay
    FiniteSequenceRelay      StableFiniteSequenceRelay MultipleRelay
    RangedSequenceRelay      MultipleToEnumerableRelay
    MultidimensionalArrayRelay  OmitArrayParameterRequestRelay
    OmitEnumerableParameterRequestRelay

(LazyRelay and StringSeedRelay do the same job from the root
CodeBrix.TestMocks.AutoFixture namespace.)

Commands (ISpecimenCommand: void Execute(object specimen, ISpecimenContext)):

    AutoPropertiesCommand, AutoPropertiesCommand<T>,
    BindingCommand<T, TProperty>, SpecifiedNullCommand<T, TProperty>,
    UnspecifiedSpecimenCommand<T>, ActionSpecimenCommand<T>,
    CompositeSpecimenCommand, ReadonlyCollectionPropertiesCommand, and
    DictionaryFiller (root namespace)

Primitive generators in CodeBrix.TestMocks.AutoFixture (add or replace them in
fixture.Customizations to change how a primitive is produced; DelegateGenerator
is the one that lives in the Kernel namespace):

    StringGenerator, ConstrainedStringGenerator, RegularExpressionGenerator,
    RandomNumericSequenceGenerator, NumericSequenceGenerator,
    RandomRangedNumberGenerator, RangedNumberGenerator,
    Int16SequenceGenerator, Int32SequenceGenerator, Int64SequenceGenerator,
    UInt16SequenceGenerator, UInt32SequenceGenerator, UInt64SequenceGenerator,
    ByteSequenceGenerator, SByteSequenceGenerator, SingleSequenceGenerator,
    DoubleSequenceGenerator, DecimalSequenceGenerator,
    CharSequenceGenerator, RandomCharSequenceGenerator,
    BooleanSwitch, RandomBooleanSequenceGenerator, GuidGenerator,
    EnumGenerator, TypeGenerator, DelegateGenerator, TaskGenerator,
    LambdaExpressionGenerator, UriGenerator, UriSchemeGenerator,
    DomainNameGenerator, EmailAddressLocalPartGenerator, MailAddressGenerator,
    RandomDateTimeSequenceGenerator, RandomDateOnlySequenceGenerator,
    RandomTimeOnlySequenceGenerator, CurrentDateTimeGenerator,
    StrictlyMonotonicallyIncreasingDateTimeGenerator, TimeZoneInfoGenerator,
    InvariantCultureGenerator, Utf8EncodingGenerator, MutableValueTypeGenerator

The graph a Fixture builds itself out of: a BehaviorRoot wraps a
TerminatingWithPathSpecimenBuilder, which wraps a composite of
CustomizationNode, AutoPropertiesTarget and ResidueCollectorNode, consulted in
that order. DefaultEngineParts and DefaultPrimitiveBuilders enumerate the
engine's builders and DefaultRelays its relays, while
SpecimenBuilderNodeAdapterCollection and
SingletonSpecimenBuilderNodeStackAdapterCollection are the adapters that make
Customizations, ResidueCollectors and Behaviors look like plain ILists over
that graph (they carry SpecimenBuilderNodeEventArgs when the graph is
replaced). TerminatingSpecimenBuilder is the plain terminator: it throws
ObjectCreationException saying the type has no usable public constructor or is
abstract; the "WithPath" variant adds the request path to the message.

Remaining kernel types you may meet in a stack trace or need when extending:
RecursionGuard and its IRecursionHandler implementations
(ThrowingRecursionHandler, OmitOnRecursionHandler, NullRecursionHandler) with
the matching ThrowingRecursionGuard / OmitOnRecursionGuard / NullRecursionGuard;
Omitter and OmitFixtureSpecification (issue an OmitSpecimen when a
specification matches); NoSpecimenOutputGuard and MutableValueTypeWarningThrower
(fail fast on bad results); IntPtrGuard (refuses IntPtr requests);
Criterion<T> with ParameterTypeAndNameCriterion, PropertyTypeAndNameCriterion
and FieldTypeAndNameCriterion (what [Frozen(Matching...)] compares with);
MemberInfoEqualityComparer; DisposableTracker; TracingBuilder, TraceWriter,
RequestTraceEventArgs and SpecimenCreatedEventArgs (the tracing plumbing);
RangedSequenceRequest; SeededFactory<T>, SpecimenFactory<T> and its
one-to-four-input overloads; IMethodFactory with
MissingParametersSupplyingMethodFactory and
MissingParametersSupplyingStaticMethodFactory;
ISpecifiedSpecimenCommand<T>; AsyncEnumeratorRelay;
ReadonlyCollectionPropertiesSpecification; and the exceptions
IllegalRequestException and TypeArgumentsCannotBeInferredException.

In the root namespace, UnwrapMemberRequest turns a member request into a plain
type request; SpecimenCommand.Do(...) and SpecimenQuery.Get(...) run an action
or a function with arguments resolved from a builder; DomainName,
EmailAddressLocalPart and UriScheme are the small value objects that the
corresponding generators produce; IRequestMemberTypeResolver /
RequestMemberTypeResolver resolve the member type behind a request (the data
annotation relays expose one as a settable property).

The three insertion points on a fixture, from first consulted to last:

    fixture.Customizations       your builders — win over everything
    (the engine)                 the default construction machinery
    fixture.ResidueCollectors    last-chance fallbacks (interfaces, etc.)
    fixture.Behaviors            wrap the whole graph (recursion, tracing)

    fixture.Customizations.Add(new ElementsBuilder<Suit>(Suit.Hearts));
    fixture.Customizations.Insert(0, myHighPriorityBuilder);
    fixture.ResidueCollectors.Add(new TypeRelay(typeof(IThing), typeof(Thing)));

Failures surface as ObjectCreationException (and the path-carrying subtype
used for recursion reports).

================================================================================

CORE API REFERENCE — AUTO-MOCKING
==================================
(CodeBrix.TestMocks.AutoFixture.AutoMock)

AutoMockCustomization teaches the fixture to satisfy a request for an
interface or abstract class by creating a mock of it, so the fixture can build
a system-under-test whose constructor dependencies are all mocks.

    public class AutoMockCustomization : ICustomization
    {
        AutoMockCustomization();
        AutoMockCustomization(ISpecimenBuilder relay);
        bool ConfigureMembers { get; set; }     // default false
        bool GenerateDelegates { get; set; }    // default false
        ISpecimenBuilder Relay { get; set; }    // default: new MockRelay()
        void Customize(IFixture fixture);
    }

    ConfigureMembers = true   every mockable member of a created mock is set
                              up to return a value taken from the fixture, so
                              a dependency's methods return populated objects
                              instead of nulls and zeros
    GenerateDelegates = true  delegate-typed dependencies are produced through
                              a mock as well

    var fixture = new Fixture();
    fixture.Customize(new AutoMockCustomization { ConfigureMembers = true });

    var mockRepo = fixture.Freeze<Mock<IOrderRepository>>();
    var processor = fixture.Create<OrderProcessor>();   // gets mockRepo.Object

AutoConfiguredMockCustomization is the older name for
"AutoMockCustomization { ConfigureMembers = true }" and is marked obsolete;
prefer the property.

Supporting public types in this namespace: MockRelay (the ISpecimenBuilder
that turns an abstraction request into a mock; its MockableSpecification
decides what counts as mockable), MockPostprocessor, MockConstructorQuery,
GreedyMockConstructorQuery, MockVirtualMethodsCommand, StubPropertiesCommand,
AutoMockPropertiesCommand, MockSealedPropertiesCommand and the MockType
extension:

    IReturnsResult<TMock> ReturnsUsingFixture<TMock, TResult>(
        this IReturns<TMock, TResult> setup, ISpecimenBuilder fixture)

    mock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsUsingFixture(fixture);

================================================================================

CORE API REFERENCE — XUNIT V3 DATA ATTRIBUTES
==============================================
(CodeBrix.TestMocks.AutoFixture.Xunit3 and
 CodeBrix.TestMocks.AutoFixture.AutoMock.Data)

All of these derive from xUnit v3's DataAttribute and feed a [Theory].

    [AttributeUsage(AttributeTargets.Method)]
    class AutoDataAttribute : DataAttribute
    {
        AutoDataAttribute();                             // uses new Fixture()
        protected AutoDataAttribute(Func<IFixture> fixtureFactory);
        Func<IFixture> FixtureFactory { get; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class InlineAutoDataAttribute : DataAttribute
    {
        InlineAutoDataAttribute(params object[] values);
        protected InlineAutoDataAttribute(Func<IFixture> fixtureFactory,
                                          params object[] values);
        object[] Values { get; }
        Func<IFixture> FixtureFactory { get; }
    }

    class MemberAutoDataAttribute : DataAttribute
    {
        MemberAutoDataAttribute(string memberName, params object[] parameters);
        MemberAutoDataAttribute(Type memberType, string memberName,
                                params object[] parameters);
        protected MemberAutoDataAttribute(Func<IFixture> fixtureFactory,
                                          string memberName,
                                          params object[] parameters);
        protected MemberAutoDataAttribute(Func<IFixture> fixtureFactory,
                                          Type memberType, string memberName,
                                          params object[] parameters);
        Type MemberType { get; }  string MemberName { get; }
        object[] Parameters { get; }  Func<IFixture> FixtureFactory { get; }
    }

    class ClassAutoDataAttribute : DataAttribute
    {
        ClassAutoDataAttribute(Type sourceType, params object[] parameters);
        protected ClassAutoDataAttribute(Func<IFixture> fixtureFactory,
                                         Type sourceType,
                                         params object[] parameters);
        Type SourceType { get; }  object[] Parameters { get; }
    }

    class CompositeDataAttribute : DataAttribute
    {
        CompositeDataAttribute(params DataAttribute[] attributes);
        CompositeDataAttribute(IEnumerable<DataAttribute> attributes);
        IReadOnlyList<DataAttribute> Attributes { get; }
    }

    class AutoMockDataAttribute : AutoDataAttribute          // AutoMock.Data
    class InlineAutoMockDataAttribute : InlineAutoDataAttribute

[AutoMockData] and [InlineAutoMockData] are exactly [AutoData] /
[InlineAutoData] with a fixture that has already been customized with
AutoMockCustomization { ConfigureMembers = true }.

    [Theory, AutoData]
    public void MyTest(string name, int id, Order order) { }

    [Theory]
    [InlineAutoData("Alice")]
    [InlineAutoData("Bob")]
    public void MyTest(string name, int autoGeneratedId) { }

    public static IEnumerable<object[]> Amounts =>
        new[] { new object[] { 10m }, new object[] { 20m } };

    [Theory]
    [MemberAutoData(nameof(Amounts))]
    public void MyTest(decimal amount, string autoGeneratedName) { }

    [Theory]
    [ClassAutoData(typeof(MyDataClass))]
    public void MyTest(int seededValue, Order order) { }

    [Theory, AutoMockData]
    public void MyTest([Frozen] Mock<IOrderRepository> repo,
                       Order order, OrderProcessor sut) { }

Inline values always come FIRST in the parameter list; the remaining
parameters are generated.

Parameter-level customization attributes
----------------------------------------

They all derive from CustomizeAttribute. The class names are the usual
"...Attribute" forms — FrozenAttribute, GreedyAttribute, ModestAttribute,
FavorArraysAttribute, FavorEnumerablesAttribute, FavorListsAttribute and
NoAutoPropertiesAttribute:

    public abstract class CustomizeAttribute
        : Attribute, IParameterCustomizationSource
    {
        public abstract ICustomization GetCustomization(ParameterInfo parameter);
    }

    [Frozen]                  freeze this parameter's value; other parameters
                              (and the SUT's constructor) get the same instance
    [Frozen(Matching by)]     control what the freeze is registered against
    [Greedy]                  build with the greediest constructor
    [Modest]                  build with the most modest constructor
    [FavorArrays]             prefer the T[] constructor
    [FavorEnumerables]        prefer the IEnumerable<T> constructor
    [FavorLists]              prefer the List<T>/IList<T> constructor
    [NoAutoProperties]        do not fill this parameter's properties

    [Flags] enum Matching
    {
        ExactType = 1, DirectBaseType = 2, ImplementedInterfaces = 4,
        ParameterName = 8, PropertyName = 16, FieldName = 32,
        MemberName = ParameterName | PropertyName | FieldName
    }

    [Theory, AutoData]
    public void Frozen_flows_into_the_graph(
        [Frozen] string customerName, Order order)
    {
        Assert.Equal(customerName, order.CustomerName);
    }

    [Theory, AutoMockData]
    public void Match_by_name(
        [Frozen(Matching.ParameterName)] IClock clock, Service sut) { }

Custom data attributes
----------------------

Subclass the attribute and pass a fixture factory to the protected
constructor. This is the supported way to share a customized fixture across
many theories:

    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMockCustomization
                              { ConfigureMembers = true });
            fixture.Customize<Order>(c => c.With(o => o.IsProcessed, true));
            fixture.RepeatCount = 5;
            return fixture;
        }
    }

    public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DomainInlineAutoDataAttribute(params object[] values)
            : base(CreateFixture, values) { }

        private static IFixture CreateFixture() { /* as above */ }
    }

    [Theory, DomainAutoData]
    public void MyTest(Order order, OrderProcessor sut) { }

    [Theory]
    [DomainInlineAutoData(42)]
    public void MyTest(int id, Order order, OrderProcessor sut) { }

You can also write a parameter attribute by deriving from CustomizeAttribute
and returning any ICustomization from GetCustomization(ParameterInfo).

The types under CodeBrix.TestMocks.AutoFixture.Xunit3.Internal (IDataSource,
DataSource, AutoDataSource, InlineDataSource, MemberDataSource,
ClassDataSource, MethodDataSource, PropertyDataSource, FieldDataSource,
ParameterMatcherBuilder) implement the attributes above. They are public only
so the attributes can share them; do not build against them.

Note: these attributes report SupportsDiscoveryEnumeration() == false, because
the argument values only exist once the fixture has run. Test explorers show
one row per theory rather than one row per generated case.

================================================================================

CORE API REFERENCE — DYNAMIC PROXY
===================================
(CodeBrix.TestMocks.DynamicProxy)

The proxy generator that the mocking API is built on is public, and can be
used directly for aspect-style interception — logging, timing, retry, lazy
loading — without any mocking involved. Roughly a hundred types are public
here; these are the ones a consumer needs.

    class ProxyGenerator : IProxyGenerator
    {
        ProxyGenerator();
        ProxyGenerator(IProxyBuilder builder);
        ProxyGenerator(bool disableSignedModule);
        ILogger Logger { get; set; }
        IProxyBuilder ProxyBuilder { get; }

        TInterface CreateInterfaceProxyWithTarget<TInterface>(
            TInterface target, params IInterceptor[] interceptors);
        TInterface CreateInterfaceProxyWithTarget<TInterface>(
            TInterface target, ProxyGenerationOptions options,
            params IInterceptor[] interceptors);
        TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(
            TInterface target, params IInterceptor[] interceptors);
        TInterface CreateInterfaceProxyWithoutTarget<TInterface>(
            params IInterceptor[] interceptors);
        TClass CreateClassProxy<TClass>(params IInterceptor[] interceptors);
        TClass CreateClassProxy<TClass>(object[] constructorArguments,
            params IInterceptor[] interceptors);
        TClass CreateClassProxy<TClass>(ProxyGenerationOptions options,
            params IInterceptor[] interceptors);
        TClass CreateClassProxyWithTarget<TClass>(TClass target,
            params IInterceptor[] interceptors);
        // plus non-generic (Type-based) overloads of each of the above, and
        // overloads taking Type[] additionalInterfacesToProxy
    }

    interface IInterceptor { void Intercept(IInvocation invocation); }

    interface IInvocation
    {
        object[] Arguments { get; }
        Type[] GenericArguments { get; }
        object InvocationTarget { get; }
        MethodInfo Method { get; }
        MethodInfo MethodInvocationTarget { get; }
        object Proxy { get; }
        object ReturnValue { get; set; }
        Type TargetType { get; }
        object GetArgumentValue(int index);
        void SetArgumentValue(int index, object value);
        MethodInfo GetConcreteMethod();
        MethodInfo GetConcreteMethodInvocationTarget();
        void Proceed();
        IInvocationProceedInfo CaptureProceedInfo();
    }

    interface IProxyGenerationHook
    {
        bool ShouldInterceptMethod(Type type, MethodInfo methodInfo);
        void NonProxyableMemberNotification(Type type, MemberInfo memberInfo);
        void MethodsInspected();
    }

    interface IInterceptorSelector
    {
        IInterceptor[] SelectInterceptors(Type type, MethodInfo method,
                                          IInterceptor[] interceptors);
    }

    class ProxyGenerationOptions
    {
        static readonly ProxyGenerationOptions Default;
        ProxyGenerationOptions();
        ProxyGenerationOptions(IProxyGenerationHook hook);
        IProxyGenerationHook Hook { get; set; }
        IInterceptorSelector Selector { get; set; }
        Type BaseTypeForInterfaceProxy { get; set; }
        IList<CustomAttributeInfo> AdditionalAttributes { get; }
        MixinData MixinData { get; }
        bool HasMixins { get; }
        void AddMixinInstance(object instance);
        void AddDelegateMixin(Delegate @delegate);
        void AddDelegateTypeMixin(Type delegateType);
        object[] MixinsAsArray();
        void Initialize();
    }

    static class ProxyUtil
    {
        bool IsProxy(object instance);
        bool IsProxyType(Type type);
        object GetUnproxiedInstance(object instance);
        Type GetUnproxiedType(object instance);
        bool IsAccessible(Type type);
        bool IsAccessible(MethodBase method);
        bool IsAccessible(MethodBase method, out string message);
        TDelegate CreateDelegateToMixin<TDelegate>(object proxy);
        Delegate CreateDelegateToMixin(object proxy, Type delegateType);
    }

Also public: AllMethodsHook (the default IProxyGenerationHook),
StandardInterceptor (calls Proceed, with overridable PreProceed / PostProceed
hooks),
AbstractInvocation, IInvocationProceedInfo, IProxyTargetAccessor,
IChangeProxyTarget, IProxyBuilder with DefaultProxyBuilder and
PersistentProxyBuilder, ModuleScope, MixinData, CustomAttributeInfo and
DynamicProxyException. Beyond those:
CodeBrix.TestMocks.DynamicProxy.Generators holds the type generators plus
AttributesToAvoidReplicating and AttributesToAlwaysReplicate (which control
which attributes are copied onto a proxy type);
...DynamicProxy.Generators.Emitters and ...Emitters.SimpleAST hold the IL
emitter; ...DynamicProxy.Contributors and ...DynamicProxy.Tokens are internal
machinery; ...DynamicProxy.Internal holds the invocation base classes
(CompositionInvocation, InheritanceInvocation,
InheritanceInvocationWithoutTarget, InterfaceMethodWithoutTargetInvocation,
TypeUtil) that generated proxies derive from; and
...DynamicProxy.Serialization holds CacheMappingsAttribute and
ProxyObjectReference. None of those are part of the everyday surface — they
are public because generated proxy types must reach them.

Only virtual/abstract members of classes, and all members of interfaces, can
be intercepted; the same limitation as mocking, for the same reason.

================================================================================

CORE API REFERENCE — REGEX-DRIVEN STRINGS
==========================================
(CodeBrix.TestMocks.Fare)

Generates strings that MATCH a regular expression (the reverse of matching).
The fixture uses it for [RegularExpression]-annotated members; it is public so
you can use it directly.

    class Xeger
    {
        Xeger(string regex);
        Xeger(string regex, Random random);
        string Generate();
    }

    var sku = new Xeger(@"[A-Z]{3}-\d{4}").Generate();   // e.g. "QWE-8412"
    var seeded = new Xeger(@"\d{3}", new Random(1234)).Generate();

Pass a seeded Random when you need reproducible values.

The underlying automaton API is public too: RegExp(string s[,
RegExpSyntaxOptions syntaxFlags]) with ToAutomaton() overloads, Automaton,
State, Transition, StatePair, BasicAutomata, BasicOperations,
SpecialOperations, MinimizationOperations, StringUnionOperations, Datatypes,
IAutomatonProvider, the [Flags] enum RegExpSyntaxOptions (Intersection,
Complement, Empty, Anystring, Automaton, Interval, All) and the
RegExpMatchingOptions constants (IgnoreCase, Singleline, Multiline,
ExplicitCapture, IgnorePatternWhitespace).

Anchors (^ and $) are stripped before generation, and the "any string"
syntax option is disabled, so use character classes and quantifiers rather
than relying on anchoring.

================================================================================

CORE API REFERENCE — LOGGING ABSTRACTION
=========================================
(CodeBrix.TestMocks.Logging)

A small logging abstraction, present because the proxy generator accepts a
logger (ProxyGenerator.Logger). It is NOT Microsoft.Extensions.Logging and is
not intended as a general-purpose logging framework for your application.

    interface ILogger
    {
        bool IsTraceEnabled { get; }  bool IsDebugEnabled { get; }
        bool IsInfoEnabled  { get; }  bool IsWarnEnabled  { get; }
        bool IsErrorEnabled { get; }  bool IsFatalEnabled { get; }
        ILogger CreateChildLogger(string loggerName);
        void Trace/Debug/Info/Warn/Error/Fatal(string message);
        void Trace/Debug/Info/Warn/Error/Fatal(Func<string> messageFactory);
        void Trace/Debug/Info/Warn/Error/Fatal(string message,
                                               Exception exception);
        void TraceFormat/DebugFormat/... (string format, params object[] args);
        void TraceFormat/DebugFormat/... (Exception exception, string format,
                                          params object[] args);
        void TraceFormat/DebugFormat/... (IFormatProvider formatProvider,
                                          string format, params object[] args);
    }

    enum LoggerLevel { Off, Fatal, Error, Warn, Info, Debug, Trace }

    NullLogger.Instance                      // does nothing (the default)
    new TraceLogger(string name)             // System.Diagnostics.Trace
    new TraceLogger(string name, LoggerLevel level)
    abstract class LevelFilteredLogger        // base for your own logger

    IExtendedLogger adds GlobalProperties / ThreadProperties (IContextProperties)
    and ThreadStacks (IContextStacks / IContextStack).

    generator.Logger = new TraceLogger("proxies", LoggerLevel.Debug);

================================================================================

COMMON USING STATEMENT COMBINATIONS
====================================

Basic mocking:

    using CodeBrix.TestMocks.Mocking;

Mocking with hand-built fixtures:

    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.AutoMock;
    using CodeBrix.TestMocks.Mocking;

Data-driven tests with auto-mocking:

    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
    using CodeBrix.TestMocks.AutoFixture.Xunit3;
    using CodeBrix.TestMocks.Mocking;

Protected member testing:

    using CodeBrix.TestMocks.Mocking;
    using CodeBrix.TestMocks.Mocking.Protected;

Writing fixture extensibility:

    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.Dsl;
    using CodeBrix.TestMocks.AutoFixture.Kernel;

Direct proxy interception:

    using CodeBrix.TestMocks.DynamicProxy;

================================================================================

COMPLETE EXAMPLES
=================

Example 1: Basic mocking
------------------------

    using CodeBrix.TestMocks.Mocking;
    using Xunit;

    public interface IOrderRepository
    {
        Order GetById(int id);
        void Save(Order order);
        void Delete(int id);
    }

    public class OrderProcessorTests
    {
        [Fact]
        public void Process_saves_the_order()
        {
            //Arrange
            var mockRepo = new Mock<IOrderRepository>();
            var order = new Order { Id = 42, CustomerName = "Alice",
                                    Total = 99.95m };
            mockRepo.Setup(r => r.GetById(42)).Returns(order);
            var processor = new OrderProcessor(mockRepo.Object);

            //Act
            processor.Process(42);

            //Assert
            mockRepo.Verify(r => r.Save(It.Is<Order>(o => o.Id == 42)),
                            Times.Once());
            mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never());
        }
    }

Example 2: Fixture plus auto-mocking, assembled by hand
-------------------------------------------------------

    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.AutoMock;
    using CodeBrix.TestMocks.Mocking;
    using Xunit;

    [Fact]
    public void Process_saves_the_generated_order()
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Customize(new AutoMockCustomization
                          { ConfigureMembers = true });

        var mockRepo = fixture.Freeze<Mock<IOrderRepository>>();
        var order = fixture.Create<Order>();
        mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);

        var processor = fixture.Create<OrderProcessor>();

        //Act
        processor.Process(order.Id);

        //Assert
        mockRepo.Verify(r => r.Save(order), Times.Once());
    }

Example 3: Data-driven test with [AutoMockData]
-----------------------------------------------

    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
    using CodeBrix.TestMocks.AutoFixture.Xunit3;
    using CodeBrix.TestMocks.Mocking;
    using Xunit;

    [Theory, AutoMockData]
    public void Process_saves_and_notifies(
        [Frozen] Mock<IOrderRepository> mockRepo,
        [Frozen] Mock<IEmailService> mockEmail,
        Order order,
        OrderProcessor sut)
    {
        //Arrange
        mockRepo.Setup(r => r.GetById(order.Id)).Returns(order);

        //Act
        sut.Process(order.Id);

        //Assert
        mockRepo.Verify(r => r.Save(order), Times.Once());
        mockEmail.Verify(e => e.SendConfirmation(order.CustomerName),
                         Times.Once());
    }

Example 4: Async mocking
------------------------

    using CodeBrix.TestMocks.Mocking;
    using Xunit;

    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task SaveAsync(Order order);
        ValueTask<int> CountAsync();
    }

    [Fact]
    public async Task ProcessAsync_saves_the_order()
    {
        //Arrange
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(42))
                .ReturnsAsync(new Order { Id = 42 });
        mockRepo.Setup(r => r.CountAsync()).ReturnsAsync(1);
        mockRepo.SetupSequence(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Order { Id = 7 })
                .ThrowsAsync(new TimeoutException());

        var processor = new OrderProcessor(mockRepo.Object);

        //Act
        await processor.ProcessAsync(42);

        //Assert
        mockRepo.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Once());
    }

Example 5: A custom ICustomization (and a data attribute that uses it)
----------------------------------------------------------------------

    using System;
    using System.Linq;
    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.AutoMock;
    using CodeBrix.TestMocks.AutoFixture.Kernel;
    using CodeBrix.TestMocks.AutoFixture.Xunit3;

    public sealed class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            // mock every abstraction the SUT asks for
            fixture.Customize(new AutoMockCustomization
                              { ConfigureMembers = true });

            // a project-specific builder (see Example 6)
            fixture.Customizations.Add(new EmailAddressBuilder());

            // last-chance mapping for an abstraction with one obvious impl
            fixture.ResidueCollectors.Add(
                new TypeRelay(typeof(IClock), typeof(SystemClock)));

            // permanent shaping of one aggregate
            fixture.Customize<Order>(c => c
                .With(o => o.IsProcessed, false)
                .Without(o => o.Id));

            fixture.RepeatCount = 5;

            // this domain has circular navigation properties
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }

    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
            => new Fixture().Customize(new DomainCustomization());
    }

    // Usage
    [Theory, DomainAutoData]
    public void Order_starts_unprocessed(Order order)
        => Assert.False(order.IsProcessed);

    // Or without the attribute:
    var fixture = new Fixture().Customize(new DomainCustomization());

Example 6: A custom ISpecimenBuilder
------------------------------------

    using System;
    using System.Reflection;
    using CodeBrix.TestMocks.AutoFixture;
    using CodeBrix.TestMocks.AutoFixture.Kernel;

    // Any string property or parameter whose name ends in "Email" gets a
    // value that actually looks like an e-mail address.
    public sealed class EmailAddressBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string name = request switch
            {
                PropertyInfo pi when pi.PropertyType == typeof(string)
                    => pi.Name,
                ParameterInfo pa when pa.ParameterType == typeof(string)
                    => pa.Name,
                _ => null
            };

            if (name == null ||
                !name.EndsWith("Email", StringComparison.OrdinalIgnoreCase))
            {
                return NoSpecimen.Instance;   // not mine — let others try
            }

            var local = context.Resolve(typeof(string));
            if (local is NoSpecimen) return local;

            return local + "@example.com";
        }
    }

    // Register it — Customizations are consulted before everything else
    var fixture = new Fixture();
    fixture.Customizations.Add(new EmailAddressBuilder());

    var customer = fixture.Create<Customer>();
    Assert.EndsWith("@example.com", customer.ContactEmail);

    // Any builder can be narrowed with a specification instead of doing the
    // filtering by hand inside Create():
    fixture.Customizations.Add(new FilteringSpecimenBuilder(
        new ElementsBuilder<string>("red", "green", "blue"),
        new PropertySpecification(typeof(string), "Colour")));

    // ...and any builder can be packaged as a customization:
    ICustomization asCustomization = new EmailAddressBuilder().ToCustomization();

    // Return new OmitSpecimen() instead of a value to leave the member unset.

Example 7: Events — subscribe, verify, raise
--------------------------------------------

    using System;
    using CodeBrix.TestMocks.Mocking;
    using Xunit;

    public interface IMonitor
    {
        event EventHandler<StatusEventArgs> StatusChanged;
        void Start();
    }

    [Fact]
    public void Watcher_subscribes_and_reacts()
    {
        //Arrange
        var mock = new Mock<IMonitor>();
        mock.SetupAdd(m => m.StatusChanged +=
                           It.IsAny<EventHandler<StatusEventArgs>>());
        mock.SetupRemove(m => m.StatusChanged -=
                              It.IsAny<EventHandler<StatusEventArgs>>());
        // raising an event as a side effect of a method call:
        mock.Setup(m => m.Start())
            .Raises(m => m.StatusChanged += null,
                    mock.Object, new StatusEventArgs("running"));

        var watcher = new Watcher(mock.Object);   // subscribes in its ctor

        //Act
        watcher.Begin();                          // calls IMonitor.Start()
        mock.Raise(m => m.StatusChanged += null,
                   mock.Object, new StatusEventArgs("stopped"));
        watcher.Dispose();                        // unsubscribes

        //Assert
        Assert.Equal(new[] { "running", "stopped" }, watcher.Seen);
        mock.VerifyAdd(m => m.StatusChanged +=
                            It.IsAny<EventHandler<StatusEventArgs>>(),
                       Times.Once());
        mock.VerifyRemove(m => m.StatusChanged -=
                               It.IsAny<EventHandler<StatusEventArgs>>(),
                          Times.Once());
    }

Example 8: A DynamicProxy interceptor
-------------------------------------

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using CodeBrix.TestMocks.DynamicProxy;
    using Xunit;

    public sealed class TimingInterceptor : IInterceptor
    {
        public List<string> Log { get; } = new List<string>();

        public void Intercept(IInvocation invocation)
        {
            var sw = Stopwatch.StartNew();
            invocation.Proceed();                 // call the real member
            sw.Stop();

            this.Log.Add($"{invocation.Method.Name} " +
                         $"({invocation.Arguments.Length} args) " +
                         $"took {sw.ElapsedMilliseconds} ms");

            if (invocation.ReturnValue is int value)
            {
                invocation.ReturnValue = value;   // could rewrite it here
            }
        }
    }

    [Fact]
    public void Interceptor_wraps_every_call()
    {
        //Arrange
        var generator = new ProxyGenerator();
        var interceptor = new TimingInterceptor();

        ICalculator proxy = generator.CreateInterfaceProxyWithTarget<ICalculator>(
            new Calculator(), interceptor);

        //Act
        int sum = proxy.Add(2, 3);

        //Assert
        Assert.Equal(5, sum);
        Assert.Single(interceptor.Log);
        Assert.True(ProxyUtil.IsProxy(proxy));
        Assert.Equal(typeof(Calculator), ProxyUtil.GetUnproxiedType(proxy));
    }

    // Choose which members are intercepted:
    var options = new ProxyGenerationOptions(new OnlyPublicMethodsHook());
    var proxy2 = generator.CreateClassProxy<Calculator>(options, interceptor);

    public sealed class OnlyPublicMethodsHook : AllMethodsHook
    {
        public override bool ShouldInterceptMethod(Type type, MethodInfo mi)
            => mi.IsPublic;
    }

    // CreateInterfaceProxyWithoutTarget has nothing to Proceed() to; an
    // interceptor for such a proxy must set invocation.ReturnValue itself.

================================================================================

MINIMUM VIABLE PROJECT
======================

A test project is an ordinary Microsoft.NET.Sdk project targeting net10.0 with
four package references. MyTests.csproj:

    <Project Sdk="Microsoft.NET.Sdk">

      <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <IsPackable>false</IsPackable>
      </PropertyGroup>

      <ItemGroup>
        <PackageReference Include="CodeBrix.TestMocks.ApacheLicenseForever"
                          Version="..." />
        <PackageReference Include="xunit.v3" Version="..." />
        <PackageReference Include="xunit.runner.visualstudio" Version="...">
          <PrivateAssets>all</PrivateAssets>
          <IncludeAssets>runtime; build; native; contentfiles; analyzers;
                         buildtransitive</IncludeAssets>
        </PackageReference>
        <PackageReference Include="Microsoft.NET.Test.Sdk" Version="..." />
      </ItemGroup>

    </Project>

(Resolve the current version of each package at the time you write the file.)

GreeterTests.cs:

    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
    using CodeBrix.TestMocks.AutoFixture.Xunit3;
    using CodeBrix.TestMocks.Mocking;
    using Xunit;

    public interface IGreeter { string Greet(string name); }

    public class Welcomer
    {
        private readonly IGreeter greeter;
        public Welcomer(IGreeter greeter) => this.greeter = greeter;
        public string Welcome(string name) => this.greeter.Greet(name) + "!";
    }

    public class GreeterTests
    {
        [Fact]
        public void Greet_returns_the_expected_message()
        {
            //Arrange
            var mock = new Mock<IGreeter>();
            mock.Setup(g => g.Greet("Alice")).Returns("Hello, Alice");

            //Act
            var actual = new Welcomer(mock.Object).Welcome("Alice");

            //Assert
            Assert.Equal("Hello, Alice!", actual);
            mock.Verify(g => g.Greet("Alice"), Times.Once());
        }

        [Theory, AutoMockData]
        public void Welcome_appends_an_exclamation_mark(
            [Frozen] Mock<IGreeter> greeter, string name, Welcomer sut)
        {
            //Arrange
            greeter.Setup(g => g.Greet(name)).Returns("hi");

            //Act & Assert
            Assert.Equal("hi!", sut.Welcome(name));
        }
    }

Then:

    dotnet build
    dotnet test

================================================================================

PERFORMANCE TIPS
================

1. PREFER [AutoMockData] OVER HAND-BUILT FIXTURES. One attribute gives you
   generated data, mocks for every dependency and a constructed SUT, with no
   fixture plumbing in the test body.

2. USE [Frozen] (or fixture.Freeze) FOR ANYTHING YOU WILL CONFIGURE OR VERIFY.
   Without it the mock you configure and the mock the SUT received are
   different instances and your Verify will fail for no visible reason.

3. KEEP MockBehavior.Loose (the default). Strict mocks throw on any call you
   did not anticipate, so every unrelated refactor breaks the test. Reach for
   Strict only when "nothing else may be called" is the actual assertion, or
   use VerifyNoOtherCalls() at the end of a loose test instead.

4. BUILD THE FIXTURE ONCE PER ATTRIBUTE, NOT PER TEST. A custom
   AutoDataAttribute passing a factory to the protected base constructor keeps
   customization cost out of every test body.

5. TURN OFF WORK YOU DO NOT NEED. NoDataAnnotationsCustomization removes the
   attribute-reflection pass; fixture.OmitAutoProperties = true (or
   Build<T>().OmitAutoProperties()) stops populating properties; and
   [NoAutoProperties] does the same for one parameter.

6. MATCH PRECISELY. It.Is<T>(x => x.Id == 42) fails at the point of the wrong
   call; It.IsAny<T>() lets a wrong call through and fails later somewhere
   less informative.

7. USE CreateMany<T>(count) AND AddManyTo INSTEAD OF LOOPS, and set
   RepeatCount once rather than passing a count everywhere.

8. USE SetupAllProperties() when you need a property bag; it is one call
   instead of one SetupProperty per property.

9. REUSE ONE ProxyGenerator when using DynamicProxy directly — it caches
   generated proxy types, and a fresh generator per call re-emits IL.

10. DO NOT ASSERT ON GENERATED VALUES' CONTENT. Anonymous data is arbitrary;
    assert on relationships (the value that went in came out) instead.

================================================================================

COMMON PITFALLS TO AVOID
========================

1. DO NOT confuse the package id with the namespaces.
   Package: CodeBrix.TestMocks.ApacheLicenseForever
   Namespaces: CodeBrix.TestMocks.Mocking, CodeBrix.TestMocks.AutoFixture, ...

2. DO NOT use the upstream namespaces (Moq, AutoFixture, AutoFixture.Xunit3,
   Castle.DynamicProxy, Fare). Only CodeBrix.TestMocks.* exists here.

3. DO NOT forget [Frozen] / Freeze when a mock must reach the SUT. This is the
   single most common cause of "Verify says the call never happened".

4. DO NOT try to mock sealed classes, static members, or non-virtual members
   of a concrete class. Only interfaces, and abstract/virtual members of
   non-sealed classes, can be intercepted.

5. DO NOT pass the Mock<T> itself to the SUT. Pass mock.Object.

6. DO NOT expect a circular object graph to just work. A default Fixture
   carries ThrowingRecursionBehavior and will throw an ObjectCreationException
   naming the circular path. Remove it and add OmitOnRecursionBehavior (or
   NullRecursionBehavior) — see Example 5.

7. DO NOT return null from ISpecimenBuilder.Create. Return NoSpecimen.Instance
   for "not my request" and new OmitSpecimen() for "leave this unset"; null is
   a legitimate specimen and will be treated as one.

8. DO NOT expect Build<T>() to change later Create<T>() calls. Build is a
   one-off pipeline; Customize<T>(...) is the persistent form.

9. DO NOT assume out/ref arguments behave like normal matchers. The value of
   an out argument in a setup is captured EAGERLY when the setup is made
   (changing the variable afterwards has no effect), and a ref argument
   matches only the same value/instance unless you use It.Ref<T>.IsAny.

10. DO NOT mix up the two IInvocation types. CodeBrix.TestMocks.Mocking's
    IInvocation describes a recorded call on a mock; DynamicProxy's
    IInvocation is the live interception context with Proceed().

11. DO NOT call invocation.Proceed() in an interceptor attached to a proxy
    created WITHOUT a target — there is nothing to proceed to. Set
    invocation.ReturnValue instead.

12. DO NOT expect realistic data. Strings are GUID-based ("Name3f2a91c8..."),
    numbers are arbitrary. If a value must look real, register a builder or
    use a [RegularExpression] annotation.

13. DO NOT use xUnit v2. These attributes derive from xUnit v3's
    DataAttribute; [Theory] must come from xunit.v3. There is no xUnit v2 or
    NUnit/MSTest support.

14. DO NOT set mock.DefaultValue = DefaultValue.Custom. It throws; Custom is
    what the property reports once you assign a DefaultValueProvider.

15. DO NOT let CodeBrix.TestMocks.Mocking.Range collide with System.Range in a
    file that uses index/range syntax — alias one of them.

16. DO NOT expect the test explorer to expand [AutoData] theories into one row
    per case before the run; discovery enumeration is not supported because
    the data does not exist until the fixture runs.

17. DO NOT treat mock.Reset() as "clear the calls". It also drops every setup,
    configured default value and registered event handler. To forget only the
    recorded calls, use mock.Invocations.Clear().

18. DO NOT use MatcherAttribute or AutoConfiguredMockCustomization in new
    code; both are obsolete. Use Match.Create and
    AutoMockCustomization { ConfigureMembers = true }.

================================================================================

WHAT THIS PACKAGE DOES NOT DO
=============================

  - It does not mock sealed classes, static classes, static methods,
    extension methods, or non-virtual instance members.
  - It does not provide a test runner or a test framework. xUnit v3 supplies
    those; this package only plugs data attributes into it.
  - It does not support xUnit v2, NUnit or MSTest.
  - It does not provide assertions. Use Assert from xUnit, or a fluent
    assertion library such as SilverAssertions.
  - It does not provide the NSubstitute or FakeItEasy API styles.
  - It does not fake HTTP, databases, the clock, or the file system. Mock your
    own abstractions over them.
  - It does not generate realistic or domain-valid data, and it is not a
    property-based testing / shrinking framework.
  - It does not do integration, UI, load or snapshot testing.
  - It does not run on .NET versions below 10, and it cannot run where runtime
    IL generation is unavailable.
  - Its CodeBrix.TestMocks.Logging types are a proxy-generator logging seam,
    not a logging framework for application code, and they are unrelated to
    Microsoft.Extensions.Logging.

================================================================================

WORKING EXAMPLES ON GITHUB
==========================

The test project is the most complete worked example of every feature. Browse
it at:

  https://github.com/ellisnet/CodeBrix.TestMocks/tree/main/tests/CodeBrix.TestMocks.Tests

Feature-to-file map (all paths below are relative to that folder):

  Setups, returns and matchers
    SetupTests.cs, SetupsTests.cs, ReturnsTests.cs, MatchersTests.cs,
    MatchTests.cs, MatchExpressionTests.cs, CustomMatcherTests.cs
  Type matchers for generic methods
    ItIsAnyTypeTests.cs, IsSubtypeTests.cs, IsValueTypeTests.cs,
    CustomTypeMatchersTests.cs, NestedTypeMatchersTests.cs
  Verification and Times
    VerifyTests.cs, TimesTests.cs, VerifiableSetupTests.cs,
    OccurrenceTests.cs
  Callbacks and delegate validation
    CallbacksTests.cs, CallbackDelegateValidationTests.cs,
    AfterReturnCallbackDelegateValidationTests.cs,
    ReturnsDelegateValidationTests.cs, ReturnsValidationTests.cs
  Async
    Async/AwaitableTests.cs, ReturnsExtensionsTests.cs,
    GeneratedReturnsExtensionsTests.cs, SetupTaskResultTests.cs,
    SequenceExtensionsTests.cs, SequentialActionExtensionsTests.cs
  Events
    MockedEventsTests.cs, EventHandlersTests.cs,
    EventHandlerTypesMustMatchTests.cs
  Invocation inspection
    InvocationsTests.cs, InterceptorTests.cs, ActionObserverTests.cs,
    MatcherObserverTests.cs
  Default values and CallBase
    DefaultValueProviderTests.cs, CustomDefaultValueProviderTests.cs,
    EmptyDefaultValueProviderTests.cs, MockDefaultValueProviderTests.cs,
    LookupOrFallbackDefaultValueProviderTests.cs, CallBaseTests.cs
  Properties, out/ref, protected, sequences, repositories
    PropertiesTests.cs, StubExtensionsTests.cs, HidePropertyTests.cs,
    OutRefTests.cs, ProtectedMockTests.cs, ProtectedAsMockTests.cs,
    MockSequenceTests.cs, MockRepositoryTests.cs, ConditionalSetupTests.cs,
    AsInterfaceTests.cs, RecursiveMocksTests.cs, MockedDelegatesTests.cs,
    CaptureTests.cs, CaptureMatchTests.cs, MockBehaviorTests.cs
  LINQ to Mocks
    Linq/QueryableMocksTests.cs, Linq/SupportedQueryingTests.cs,
    Linq/MockRepositoryQueryingTests.cs
  Fixture basics and customizations
    AutoFixture/FixtureTest.cs, AutoFixture/CompositeCustomizationTest.cs,
    AutoFixture/FreezeOnMatchCustomizationTest.cs,
    AutoFixture/FixtureFreezerTest.cs, AutoFixture/FixtureRegistrarTest.cs,
    AutoFixture/CollectionFillerTest.cs,
    AutoFixture/CustomizationExtensionsTest.cs
  Recursion behaviors and circular graphs
    AutoFixture/OmitOnRecursionBehaviorTest.cs,
    AutoFixture/AbstractRecursionIssue/Repro.cs,
    AutoFixture/NavigationPropertyRecursionIssue/Repro.cs,
    AutoFixture/StaticFactoryWithClosureOfOperationsBugRepro.cs
  The kernel (custom builders, specifications, relays)
    AutoFixture/Kernel/ (including CompositeSpecimenBuilderTest.cs,
    FilteringSpecimenBuilderTest.cs, PostprocessorTest.cs,
    TypeRelayTests.cs, SpecimenBuilderNodeFactoryTests.cs)
  The Build<T>() DSL
    AutoFixture/Dsl/NodeComposerTest.cs,
    AutoFixture/Dsl/CompositePostprocessComposerTest.cs
  Data annotations
    AutoFixture/DataAnnotations/ (RangeAttributeRelayTest.cs,
    StringLengthAttributeRelayTest.cs, MinAndMaxLengthAttributeRelayTest.cs,
    RegularExpressionAttributeRelayTest.cs, EnumDataTypeAttributeRelayTest.cs)
  Auto-mocking
    AutoFixture/AutoMock/AutoMockCustomizationTest.cs,
    AutoFixture/AutoMock/FixtureIntegrationWithAutoMockCustomizationTest.cs,
    AutoFixture/AutoMock/MockRelayTest.cs,
    AutoFixture/AutoMock/MockVirtualMethodsCommandTest.cs,
    AutoFixture/AutoMock/MockTypeTest.cs
  xUnit v3 data attributes
    AutoFixture/Xunit3/AutoDataAttributeTest.cs,
    AutoFixture/Xunit3/InlineAutoDataAttributeTests.cs,
    AutoFixture/Xunit3/MemberAutoDataAttributeTest.cs,
    AutoFixture/Xunit3/ClassAutoDataAttributeTests.cs,
    AutoFixture/Xunit3/FrozenAttributeTest.cs,
    AutoFixture/Xunit3/Scenario.cs
  Proxy generation
    ProxyFactories/MostSpecificOverrideTests.cs,
    Regressions/IssueReportsTests.cs

A full URL is the folder URL above plus "/" plus the path, for example:
  .../tree/main/tests/CodeBrix.TestMocks.Tests/AutoFixture/Xunit3/Scenario.cs

================================================================================

QUICK REFERENCE CARD
====================

Install:          dotnet add package CodeBrix.TestMocks.ApacheLicenseForever
Target:           .NET 10 or later          License: Apache-2.0
Dependency:       xunit.v3.extensibility.core

Namespaces
  Mocking          using CodeBrix.TestMocks.Mocking;
  Protected        using CodeBrix.TestMocks.Mocking.Protected;
  Fixtures         using CodeBrix.TestMocks.AutoFixture;
  Kernel           using CodeBrix.TestMocks.AutoFixture.Kernel;
  Build<T>() DSL   using CodeBrix.TestMocks.AutoFixture.Dsl;
  Auto-mocking     using CodeBrix.TestMocks.AutoFixture.AutoMock;
  xUnit3 data      using CodeBrix.TestMocks.AutoFixture.Xunit3;
  AutoMock data    using CodeBrix.TestMocks.AutoFixture.AutoMock.Data;
  Proxies          using CodeBrix.TestMocks.DynamicProxy;
  Regex strings    using CodeBrix.TestMocks.Fare;

Mocking
  Create           new Mock<IService>()  /  new Mock<IService>(MockBehavior.Strict)
  Instance         mock.Object
  Setup            mock.Setup(s => s.Method(args)).Returns(value)
  Setup async      mock.Setup(s => s.M()).ReturnsAsync(value)
  Throw            .Throws<TException>() / .Throws(ex) / .ThrowsAsync(ex)
  Callback         .Callback<T>(arg => ...)
                   .Callback(new InvocationAction(i => ...))
  Sequence         mock.SetupSequence(s => s.M()).Returns(v1).Returns(v2)
  Properties       mock.SetupProperty(s => s.P[, initial]) / SetupAllProperties()
  Events           mock.SetupAdd/SetupRemove/VerifyAdd/VerifyRemove
                   mock.Raise(s => s.E += null, args) / RaiseAsync(...)
                   .Raises(s => s.E += null, args)
  Verify           mock.Verify(s => s.M(args), Times.Once())
                   mock.VerifyGet / VerifySet / VerifyAll / Verify /
                   VerifyNoOtherCalls
  Times            Once/Never/AtLeastOnce/AtLeast/AtMostOnce/AtMost/Exactly/Between
  Matchers         It.IsAny<T>() / It.Is<T>(x => c) / It.IsIn / It.IsNotIn /
                   It.IsInRange / It.IsRegex / It.IsNotNull<T>()
  Type matchers    It.IsAnyType / It.IsSubtype<T> / It.IsValueType / ITypeMatcher
  Custom matcher   Match.Create<T>(v => predicate)
  Ref/out          It.Ref<T>.IsAny
  Capture          Capture.In(list) / Capture.In(list, pred) / Capture.With(match)
  Defaults         mock.DefaultValue = DefaultValue.Mock
                   mock.DefaultValueProvider = new MyProvider()
                   mock.SetReturnsDefault<bool>(true)
  Call real code   mock.CallBase = true  /  setup.CallBase()
  Inspect          mock.Invocations / mock.Setups / mock.Invocations.Clear()
  Extra interface  mock.As<IDisposable>()
  Conditional      mock.When(() => flag).Setup(...)
  Ordering         mock1.InSequence(new MockSequence()).Setup(...)
  Repository       new MockRepository(MockBehavior.Strict).Create<T>()
  Protected        mock.Protected().Setup<TResult>("Name", ItExpr.IsAny<int>())
                   mock.Protected().As<IAnalog>().Setup(a => a.M(...))
  LINQ             Mock.Of<IService>(s => s.Name == "x")  /  Mocks.Of<T>()
  From instance    Mock.Get(instance)
  Reset            mock.Reset()
  Failure type     MockException (IsVerificationError)

Test data
  Create           fixture.Create<T>()  /  fixture.CreateMany<T>([count])
  Shape once       fixture.Build<T>().With(x => x.P, v).Without(x => x.Q).Create()
  Shape always     fixture.Customize<T>(c => c.With(x => x.P, v))
  Share instance   fixture.Freeze<T>()  /  fixture.Inject(instance)
  Factory          fixture.Register<T>(() => new T(...))
  Fill collection  fixture.AddManyTo(collection[, count])
  Endless supply   new Generator<T>(fixture)
  Knobs            fixture.RepeatCount / fixture.OmitAutoProperties
  Extend           fixture.Customizations / .ResidueCollectors / .Behaviors
  Customization    class X : ICustomization { void Customize(IFixture f) }
                   fixture.Customize(new X())
                   new CompositeCustomization(a, b, c)
  Builder          class B : ISpecimenBuilder
                   { object Create(object request, ISpecimenContext context) }
                   return NoSpecimen.Instance / new OmitSpecimen()
  Recursion        remove ThrowingRecursionBehavior, add
                   OmitOnRecursionBehavior
  Annotations      [Range], [StringLength], [MinLength], [MaxLength],
                   [RegularExpression], [EnumDataType] are honoured
  Auto-mocking     fixture.Customize(new AutoMockCustomization
                                     { ConfigureMembers = true })

xUnit v3 attributes
  [Theory, AutoData]                all parameters generated
  [Theory, AutoMockData]            generated + mocks + constructed SUT
  [InlineAutoData(v1, v2)]          leading inline values, rest generated
  [InlineAutoMockData(v1)]          same, with mocking
  [MemberAutoData(nameof(Source))]  member data + generated
  [ClassAutoData(typeof(Source))]   class data + generated
  [Frozen] / [Frozen(Matching.X)]   share this parameter's instance
  [Greedy] / [Modest]               constructor choice
  [FavorArrays] / [FavorEnumerables] / [FavorLists]
  [NoAutoProperties]                do not fill properties
  Custom attribute                  : AutoDataAttribute, base(fixtureFactory)

Proxies
  var g = new ProxyGenerator();
  g.CreateInterfaceProxyWithTarget<T>(target, interceptor)
  g.CreateInterfaceProxyWithoutTarget<T>(interceptor)
  g.CreateClassProxy<T>([options,] [ctorArgs,] interceptor)
  class I : IInterceptor { void Intercept(IInvocation inv) }
  inv.Proceed() / inv.ReturnValue / inv.Arguments / inv.Method
  ProxyUtil.IsProxy(o) / ProxyUtil.GetUnproxiedType(o)

Regex-driven strings
  new Xeger(@"[A-Z]{3}-\d{4}").Generate()

================================================================================
