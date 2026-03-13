namespace CodeBrix.TestMocks.Tests.AutoFixture.AbstractRecursionIssue; //was previously: namespace AutoFixtureUnitTest.AbstractRecursionIssue;

public class ItemLocation
{
    public int LocationId { get; set; }

    public ItemBase Item { get; set; }
}