using System.Collections.Generic;
using System.Linq;
using CodeBrix.TestMocks.AutoFixture;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture; //was previously: namespace AutoFixtureUnitTest;

public class ListGeneratorTest
{
    [Fact]
    public void AddManyWillAddItemsToList()
    {
        // Arrange
        int anonymousCount = 5;
        IEnumerable<int> expectedList = Enumerable.Range(1, anonymousCount);
        List<int> list = new List<int>();
        // Act
        int i = 0;
        list.AddMany(() => ++i, anonymousCount);
        // Assert
        Assert.True(expectedList.SequenceEqual(list));
    }

    [Fact]
    public void AddManyWillAddItemsToCollection()
    {
        // Arrange
        int anonymousCount = 8;
        IEnumerable<int> expectedSequence = Enumerable.Range(1, anonymousCount);
        ICollection<int> collection = new LinkedList<int>();
        // Act
        int i = 0;
        collection.AddMany(() => ++i, anonymousCount);
        // Assert
        Assert.True(expectedSequence.SequenceEqual(collection));
    }
}