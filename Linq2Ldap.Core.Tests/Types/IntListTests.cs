using Xunit;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.Core.Tests.Types {
    public class IntListTests {
        [Fact]
        public void NullComparisons_FalseButWhenNotEq() {
            Assert.False((IntList)null == 0);
            Assert.False((IntList)null < 0);
            Assert.False((IntList)null > 0);
            Assert.False((IntList)null >= 0);
            Assert.False((IntList)null <= 0);
            Assert.True((IntList)null != 0);
        }

        [Fact]
        public void EmptyComparisons_FalseButWhenNotEq() {
            var empty = new IntList();
            Assert.False(empty == 0);
            Assert.True(empty != 0);
            Assert.False(empty < 0);
            Assert.False(empty > 0);
            Assert.False(empty <= 0);
            Assert.False(empty >= 0);
        }

        [InlineData(new [] { 1, 2 }, 2, true)]
        [InlineData(new [] { 1, 2 }, 1, true)]
        [InlineData(new [] { 1, 2 }, 3, false)]
        [Theory]
        public void Eq_ValidWhenAny(int[] arr, int i, bool expected) {
            Assert.Equal(expected, new IntList(arr) == i);
        }

        [InlineData(new [] { 1, 2 }, 2, false)]
        [InlineData(new [] { 1, 2 }, 1, false)]
        [InlineData(new [] { 1, 2 }, 3, true)]
        [Theory]
        public void NotEq_ValidWhenAny(int[] arr, int i, bool expected) {
            Assert.Equal(expected, new IntList(arr) != i);
        }

        [InlineData(new [] { 1, 2 }, 2, false)]
        [InlineData(new [] { 1 }, 1, false)]
        [InlineData(new [] { 1, 2, 3 }, 2, true)]
        [Theory]
        public void Greater_WhenAny(int[] arr, int i, bool expected) {
            Assert.Equal(expected, new IntList(arr) > i);
        }

        [InlineData(new [] { 1, 2 }, 2, true)]
        [InlineData(new [] { 1 }, 1, false)]
        [InlineData(new [] { 1, 2 }, 3, true)]
        [Theory]
        public void Less_WhenAny(int[] arr, int i, bool expected) {
            Assert.Equal(expected, new IntList(arr) < i);
        }

        [InlineData(new [] { 1, 2 }, 0, false)]
        [InlineData(new [] { 1 }, 1, true)]
        [InlineData(new [] { 1, 2 }, 2, true)]
        [Theory]
        public void LessOrEq_WhenAny(int[] arr, int i, bool expected) {
            Assert.Equal(expected, new IntList(arr) <= i);
        }

        [InlineData(new [] { 1, 2 }, 0, true)]
        [InlineData(new [] { 1 }, 1, true)]
        [InlineData(new [] { 1, 2 }, 3, false)]
        [Theory]
        public void GreaterOrEq_WhenAny(int[] arr, int i, bool expected) {
            Assert.Equal(expected, new IntList(arr) >= i);
        }
    }
}