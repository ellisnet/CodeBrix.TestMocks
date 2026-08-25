================================================================================
MAINTAINER-README: CodeBrix.TestMocks
Notes for people and agents MAINTAINING this repository — not for package
consumers
================================================================================

If you are CONSUMING the NuGet package, read AGENT-README.txt instead. This
file is about the repository itself.

PURPOSE AND SCOPE
=================

The repository produces exactly one NuGet package:

    CodeBrix.TestMocks.ApacheLicenseForever
        Project:       src/CodeBrix.TestMocks/CodeBrix.TestMocks.csproj
        Assembly:      CodeBrix.TestMocks
        Consumer doc:  AGENT-README.txt (repo root)

The assembly consolidates the source of several upstream testing libraries
into one assembly under CodeBrix.TestMocks.* namespaces, so that a consumer
needs a single package reference — and a single license — for mocking, dynamic
proxy generation, test-data generation and xUnit v3 data attributes.

The "ApacheLicenseForever" suffix in the package id is a promise: the package
stays Apache-2.0. Do not rename the package or change
PackageLicenseExpression.

REPOSITORY LAYOUT
=================

    AGENT-README.txt            consumer documentation (ships in the nupkg)
    MAINTAINER-README.txt       this file
    EXTRAS-README.txt           non-package content in the repo
    README-INDEX.txt            map of the README files
    README.md                   human-facing overview (GitHub + nuget.org)
    LICENSE                     Apache-2.0
    THIRD-PARTY-NOTICES.txt     upstream attribution (ships in the nupkg)
    icon-codebrix-128.png       package icon (ships in the nupkg)
    CodeBrix.TestMocks.slnx     solution
    AGENTS.md, CLAUDE.md, .clinerules, .cursorrules, .windsurfrules,
    .cursor/rules/agent-readme.mdc, .github/copilot-instructions.md,
    .junie/guidelines.md        AI-agent pointer stubs; they only point at
                                AGENT-README.txt and are maintained centrally
                                across the CodeBrix repos — do not edit them
                                here

    src/CodeBrix.TestMocks/
        Mocking/                mocking API (Async, Behaviors, Expressions,
                                Interception, Language, Language/Flow, Linq,
                                Matchers, Obsolete, Properties, Protected)
        AutoFixture/            test-data API
            AutoMock/           auto-mocking glue (+ AutoMock/Data attributes)
            DataAnnotations/    data-annotation relays
            Dsl/                the Build<T>() composer types
            Kernel/             specimen builders, requests, specifications
            Xunit3/             xUnit v3 data attributes (+ Xunit3/Internal)
        DynamicProxy/           proxy generator (Contributors, Generators,
                                Generators/Emitters, Internal, Serialization,
                                Tokens)
        Fare/                   regex -> string generation
        Logging/                logging abstraction used by DynamicProxy
        Helpers/                SynchronizedDictionary, TypeExtensions,
                                TypeNameFormatter
        InternalsVisibleTo.cs   grants access to CodeBrix.TestMocks.Tests

    tests/CodeBrix.TestMocks.Tests/
        The upstream test suites, ported alongside the sources, mirroring the
        src folder structure (AutoFixture/, AutoFixture/Kernel/, Linq/,
        Matchers/, Regressions/, ProxyFactories/, Helpers/, Async/).

The solution file lists .gitignore, AGENT-README.txt, icon-codebrix-128.png,
LICENSE, README.md and THIRD-PARTY-NOTICES.txt under "Solution Items" and the
test project under a "Tests" folder. If you add a root-level document that
maintainers should see in the IDE, add it to Solution Items too.

BUILDING
========

    dotnet restore CodeBrix.TestMocks.slnx
    dotnet build   CodeBrix.TestMocks.slnx

Both projects target net10.0 only, and nothing else. Do not add extra target
frameworks: the family rule is net10.0-only (netstandard is reserved for
Roslyn analyzer hosts, which this repository does not have).

GeneratePackageOnBuild is true on the library project, so every build of
src/CodeBrix.TestMocks produces a .nupkg — see PACKAGING AND PUBLISHING for
why that matters.

Conditional compilation symbols appearing in the vendored sources
(FEATURE_SERIALIZATION, SYSTEM_RUNTIME_SERIALIZATION,
FEATURE_ASSEMBLYBUILDER_SAVE, FEATURE_APPDOMAIN,
FEATURE_DEFAULT_INTERFACE_IMPLEMENTATIONS, SYSTEM_NET_MAIL,
TYPENAMEFORMATTER_INTERNAL, TYPENAMEFORMATTER_USE_SEMIBROKEN_REFLECTION) are
NOT defined by the project file. They are upstream multi-targeting leftovers
and the code inside them is intentionally excluded. Leave them in place rather
than deleting the guarded blocks, so future upstream diffs still apply.

TESTING
=======

    dotnet test CodeBrix.TestMocks.slnx

The test project references xunit.v3, xunit.runner.visualstudio and
Microsoft.NET.Test.Sdk and defines SYSTEM_THREADING_THREAD_CULTURESETTERS in
both Debug and Release. It reaches internals through the InternalsVisibleTo
attribute in src/CodeBrix.TestMocks/InternalsVisibleTo.cs, so the test
assembly name must stay CodeBrix.TestMocks.Tests.

There are no opt-in environment variables, no external services and no
platform-specific tests; the whole suite is pure managed code and runs
anywhere .NET 10 runs.

The test project is also the reference documentation for the library's
behavior: the AGENT-README "WORKING EXAMPLES ON GITHUB" section maps features
to files in it. When you add a feature area, add the map entry too.

PACKAGING AND PUBLISHING
========================

Versioning: date-stamped and auto-incrementing, computed in
src/CodeBrix.TestMocks/CodeBrix.TestMocks.csproj from System.DateTime.UtcNow
as 1.<years since _VersionBaseYear>.<day of year>.<minute of day>. Version,
AssemblyVersion and FileVersion all take that value. Consequences:

  - every build produces a new version, and with GeneratePackageOnBuild that
    means a new .nupkg on every build;
  - two builds within the same UTC minute produce the SAME version, so never
    publish two packages from within one minute;
  - it is not SemVer — major is pinned to 1 and minor encodes the year, so
    the numbers say nothing about API compatibility;
  - to re-baseline the minor number, change _VersionBaseYear.

What ships inside the nupkg, in addition to the assembly:

    icon-codebrix-128.png     (PackageIcon)
    README.md                 (PackageReadmeFile)
    AGENT-README.txt          consumer documentation
    THIRD-PARTY-NOTICES.txt   upstream attribution

All four are pulled from the repository root by <None Include="..\..\..."/>
items in the library csproj. If AGENT-README.txt is renamed or split, update
that item group, or consumers silently lose the documentation.

Package metadata to keep stable: PackageId
(CodeBrix.TestMocks.ApacheLicenseForever), Product/Title (CodeBrix.TestMocks),
Authors (Jeremy Ellis), Copyright ("Copyright (c) 2026 Jeremy Ellis and
contributors"), PackageLicenseExpression (Apache-2.0),
PackageRequireLicenseAcceptance (true), and the project/repository URLs
(https://github.com/ellisnet/CodeBrix.TestMocks).

The only NuGet dependency is xunit.v3.extensibility.core, which the xUnit v3
data attributes compile against. Keep the dependency surface this small:
consumers who want mocking without xUnit still take this one reference, and
anything more would leak into every test project in the family.

PROVENANCE AND VENDORED SOURCES
===============================

All source in src/CodeBrix.TestMocks is vendored — there are no upstream
package references. THIRD-PARTY-NOTICES.txt is the authoritative attribution
document; keep it in sync with the folder list below.

    src/CodeBrix.TestMocks/Mocking/
        Moq (https://github.com/devlooped/moq), BSD 3-Clause.
        Original namespaces: Moq, Moq.Language, Moq.Language.Flow, Moq.Linq,
        Moq.Protected. IFluentInterface.cs is separately Apache-2.0.

    src/CodeBrix.TestMocks/AutoFixture/
        AutoFixture (https://github.com/AutoFixture/AutoFixture), MIT.
        Original namespaces: AutoFixture, AutoFixture.Kernel, AutoFixture.Dsl,
        AutoFixture.DataAnnotations, AutoFixture.Xunit3, and — for the
        AutoMock folder — AutoFixture.AutoMoq.

    src/CodeBrix.TestMocks/DynamicProxy/,
    src/CodeBrix.TestMocks/Logging/,
    src/CodeBrix.TestMocks/Helpers/SynchronizedDictionary.cs,
    src/CodeBrix.TestMocks/Helpers/TypeExtensions.cs
        Castle.Core / DynamicProxy (https://github.com/castleproject/Core),
        Apache-2.0. Original namespaces: Castle.DynamicProxy.*, Castle.Core.*.

    src/CodeBrix.TestMocks/Fare/
        Fare (https://github.com/moodmosaic/Fare), BSD 3-Clause and
        Apache-2.0. Original namespace: Fare.

    src/CodeBrix.TestMocks/Helpers/TypeNameFormatter.cs
        TypeNameFormatter (https://github.com/stakx/TypeNameFormatter), MIT.

Every renamed namespace declaration carries a trailing comment recording what
it used to be, for example:

    namespace CodeBrix.TestMocks.Mocking; //was previously: namespace Moq;

Keep that convention when you touch a file, and keep the upstream copyright
headers at the top of each file intact. They are what makes the attribution in
THIRD-PARTY-NOTICES.txt verifiable.

Pulling in an upstream change: diff against the upstream file, apply the
change, then re-apply the namespace rename and the file-scoped namespace
conversion. Do not "modernize" vendored code beyond that — the smaller the
delta, the cheaper the next upstream merge.

CODING CONVENTIONS
==================

  - net10.0 only, for every project in the repository.
  - File-scoped namespaces; the "//was previously:" comment stays on the
    namespace line of vendored files.
  - Nullable reference types are OFF at project level (the family default —
    the csproj sets no <Nullable> property). Do not add "?" annotations to
    reference types in ordinary files. Four vendored files opt in locally with
    a "#nullable enable" directive on line 1 (AutoFixture/Xunit3/
    MemberAutoDataAttribute.cs and three files under
    AutoFixture/Xunit3/Internal/); keep the directive if you edit them, and
    keep the "!" null-forgiving operators that some DynamicProxy files
    inherited from upstream.
  - XML documentation comments are expected on public members; fix CS1591 at
    the source rather than suppressing it. Vendored files arrive with upstream
    documentation — preserve it.
  - Public API shape follows upstream. When upstream marks something
    [Obsolete] or [EditorBrowsable(Never)], keep the attribute; several types
    here (MockFactory, MatcherAttribute, IOccurrence,
    AutoConfiguredMockCustomization, the Mocking/Obsolete folder as a whole)
    exist only for source compatibility.
  - Test files are named <Class>Tests.cs or, where the port inherited it,
    <Class>Test.cs; test method names are snake_case in newly written tests,
    and bodies use //Arrange, //Act, //Assert comments. Do not rename the
    ported upstream test files wholesale.
  - Documentation in this repository names upstream projects by their real
    names (Moq, AutoFixture, Castle DynamicProxy, Fare) or refers to "the
    upstream project"; it does not carry over naming from unrelated
    frameworks.

NOTES
=====

  - AGENT-README.txt is a packaged artifact, not just a repo document. Treat
    an edit to it as an edit to the product.
  - The two IInvocation interfaces (Mocking and DynamicProxy) are a known
    source of confusion; if you add documentation touching either, say which
    one you mean.
  - Mocking/Interception/CastleProxyFactory.cs is the seam between the mocking
    API and the vendored proxy generator; it caches one ProxyGenerator per
    namespace. Changes there affect every mock in every consuming project.
  - There is no build script, CI workflow or pack driver in this repository;
    packing is whatever "dotnet build" of the library project produces.
