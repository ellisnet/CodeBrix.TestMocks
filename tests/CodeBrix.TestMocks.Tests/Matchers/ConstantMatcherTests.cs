using CodeBrix.TestMocks.Mocking;

using Xunit;

namespace CodeBrix.TestMocks.Tests.Matchers;

public class ConstantMatcherTests
{
    public class No_infinite_recursion
    {
        [Fact]
        public void Can_match_some_object_against_mock_object_if_mock_has_setup_for_Equals_its_object()
        {
            var mock = new Mock<object>();
            mock.Setup(m => m.Equals(mock.Object)).Returns(true);
            Assert.False(mock.Object.Equals(new object()));
        }

        [Fact]
        public void Can_match_mock_object_against_itself_if_mock_has_setup_for_Equals_its_object()
        {
            var mock = new Mock<object>();
            mock.Setup(m => m.Equals(mock.Object)).Returns(true);
            Assert.True(mock.Object.Equals(mock.Object));
        }

        [Fact]
        public void Can_match_mock_object_against_another_mock_object_if_other_mock_has_setup_for_Equals_its_object()
        {
            var mockA = new Mock<object>();
            var mockB = new Mock<object>();
            mockA.Setup(m => m.Equals(mockA.Object)).Returns(true);
            mockB.Setup(m => m.Equals(mockB.Object)).Returns(true);
            Assert.True(mockA.Object.Equals(mockA.Object));
            Assert.False(mockA.Object.Equals(mockB.Object));
            Assert.False(mockB.Object.Equals(mockA.Object));
            Assert.True(mockB.Object.Equals(mockB.Object));
        }

        [Fact]
        public void Workaround_1()
        {
            var mockA = new Mock<object>();
            var mockB = new Mock<object>();
            Assert.True(mockA.Object.Equals(mockA.Object));
            Assert.False(mockA.Object.Equals(mockB.Object));
            Assert.False(mockB.Object.Equals(mockA.Object));
            Assert.True(mockB.Object.Equals(mockB.Object));
        }

        [Fact]
        public void Workaround_2()
        {
            var mockA = new Mock<object>();
            var mockB = new Mock<object>();
            mockA.Setup(m => m.Equals(It.Is<object>(x => object.ReferenceEquals(x, mockA.Object)))).Returns(true);
            mockB.Setup(m => m.Equals(It.Is<object>(x => object.ReferenceEquals(x, mockB.Object)))).Returns(true);
            Assert.True(mockA.Object.Equals(mockA.Object));
            Assert.False(mockA.Object.Equals(mockB.Object));
            Assert.False(mockB.Object.Equals(mockA.Object));
            Assert.True(mockB.Object.Equals(mockB.Object));
        }
    }
}
