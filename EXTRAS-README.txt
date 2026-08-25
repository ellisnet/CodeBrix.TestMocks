================================================================================
EXTRAS-README: CodeBrix.TestMocks
Samples, tools and other content in this repository that is not part of a
NuGet package
================================================================================

This repository ships no samples, demo applications, tools or optional test
data. Everything under src/ becomes the single NuGet package
(CodeBrix.TestMocks.ApacheLicenseForever); the only other content is the test
project.

TEST PROJECT
============

    tests/CodeBrix.TestMocks.Tests/

What it is
----------
The unit-test suite for the library, ported alongside the vendored upstream
sources it covers. It is not packaged and not published; it is a test project
in the solution, and it doubles as the most complete set of worked examples
for the library's behavior.

Its folder structure mirrors src/CodeBrix.TestMocks: mocking tests sit at the
root, with Async/, Linq/, Matchers/, Helpers/, ProxyFactories/ and
Regressions/ subfolders, and the test-data suites live under AutoFixture/
(with AutoMock/, DataAnnotations/, Dsl/, Kernel/, Xunit3/ and several
recursion-issue reproduction folders).

How to run it
-------------

    dotnet test CodeBrix.TestMocks.slnx

or, from the test project folder:

    dotnet test

There is nothing for you to configure: no environment variables, no external
services, no platform-specific prerequisites. The one piece of required
configuration, global.json (which selects the Microsoft Testing Platform
runner), is committed at the repository root, so a fresh clone runs the suite
as-is.

Do NOT add --nologo to either command. It is a VSTest-only switch, and in
Microsoft Testing Platform mode the .NET 10 SDK forwards it to the test
application, which rejects it and exits before discovery. The run then reports
"Zero tests ran" (exit code 5) with nothing explaining why, which reads like a
broken test suite rather than a bad argument. When you want the banner
suppressed, use the MTP spelling instead:

    dotnet test CodeBrix.TestMocks.slnx -- --no-banner

MAINTAINER-README.txt has the background and the upstream issue numbers.

What it demonstrates
--------------------
Every feature area of the package, usually with one file per feature. The
"WORKING EXAMPLES ON GITHUB" section of AGENT-README.txt maps features to the
individual files, and is the right place to start when the documentation does
not answer a question.

A TestResults/ folder may appear inside the test project after a run with
coverage collection; it is generated output and is not part of the
repository's content.
