using System.Collections.Generic;

namespace CodeBrix.TestMocks.Tests.AutoFixture.NavigationPropertyRecursionIssue; //was previously: namespace AutoFixtureUnitTest.NavigationPropertyRecursionIssue;

public class Language
{
    public ICollection<Session> Sessions { get; set; }
}