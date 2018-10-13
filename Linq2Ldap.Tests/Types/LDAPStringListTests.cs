using Linq2Ldap.Types;
using Xunit;

namespace Linq2Ldap.Tests.Types {
    public class LDAPStringListTests {
        [InlineData(new [] { "abc", "cde" }, "cde", true)]
        [InlineData(new [] { "abc", "cde" }, "efg", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void Equals_AnyEquals_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test == toFind;
            Assert.Equal(expected, actual);
        }

        [InlineData(null, true)]
        [InlineData("something", false)]
        [Theory]
        public void Equals_LeftNull_FalseIfRightNotNull(string right, bool expected) {
            LDAPStringList test = null;
            var actual = test == right;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "cde", false)]
        [InlineData(new [] { "abc", "cde" }, "efg", true)]
        [InlineData(new string[] { }, "efg", true)]
        [Theory]
        public void NotEquals_AnyEquals_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test != toFind;
            Assert.Equal(expected, actual);
        }

        [InlineData(null, false)]
        [InlineData("something", true)]
        [Theory]
        public void NotEquals_LeftNull_FalseIfRightNotNull(string right, bool expected) {
            LDAPStringList test = null;
            var actual = test != right;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "cde", true)]
        [InlineData(new [] { "abc", "cde" }, "abc", true)]
        [InlineData(new [] { "abc", "cde" }, "efg", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void GreaterThanEqual_AnyGTE_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test >= toFind;
            Assert.Equal(expected, actual);
        }

        [InlineData(null, true)]
        [InlineData("something", false)]
        [Theory]
        public void GreaterThanEqual_LeftNull_FalseIfRightNotNull(string right, bool expected) {
            LDAPStringList test = null;
            var actual = test >= right;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "abc", true)]
        [InlineData(new [] { "abc", "cde" }, "123", false)]
        [InlineData(new [] { "abc", "cde" }, "efg", true)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void LessThanEqual_AnyLTE_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test <= toFind;
            Assert.Equal(expected, actual);
        }

        [InlineData(null, true)]
        [InlineData("something", false)]
        [Theory]
        public void LessThanEqual_LeftNull_FalseIfRightNotNull(string right, bool expected) {
            LDAPStringList test = null;
            var actual = test <= right;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "cde", true)]
        [InlineData(new [] { "abc", "cde" }, "abc", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void LessThan_AnyLessThan_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test < toFind;
            Assert.Equal(expected, actual);
        }

        [InlineData(null, true)]
        [InlineData("something", false)]
        [Theory]
        public void LessThan_LeftNull_FalseIfRightNotNull(string right, bool expected) {
            LDAPStringList test = null;
            var actual = test < right;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "cde", false)]
        [InlineData(new [] { "abc", "cde" }, "abc", true)]
        [InlineData(new [] { "abc", "cde" }, "123", true)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void GreaterThan_AnyLessThan_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test > toFind;
            Assert.Equal(expected, actual);
        }

        [InlineData(null, true)]
        [InlineData("something", false)]
        [Theory]
        public void GreaterThan_LeftNull_FalseIfRightNotNull(string right, bool expected) {
            LDAPStringList test = null;
            var actual = test > right;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "ab", true)]
        [InlineData(new [] { "abc", "cde" }, "c", true)]
        [InlineData(new [] { "abc", "cde" }, "gf", false)]
        [InlineData(new [] { "abc", "cde" }, "bc", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void StartsWith_Any_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test.StartsWith(toFind);
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "de", true)]
        [InlineData(new [] { "abc", "cde" }, "bc", true)]
        [InlineData(new [] { "abc", "cde" }, "gf", false)]
        [InlineData(new [] { "abc", "cde" }, "ab", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void EndsWith_Any_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test.EndsWith(toFind);
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "de", true)]
        [InlineData(new [] { "abc", "cde" }, "ab", true)]
        [InlineData(new [] { "abc", "cde" }, "d", true)]
        [InlineData(new [] { "abc", "cde" }, "ac", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void Contains_Any_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test.Contains(toFind);
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "abc", true)]
        [InlineData(new [] { "abc", "cde" }, "ab", false)]
        [InlineData(new [] { "abc", "cde" }, "cde", true)]
        [InlineData(new [] { "abc", "cde" }, "efg", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void CompareTo_Eq_Any_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test.CompareTo(toFind) == 0;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "abc", true)]
        [InlineData(new [] { "abc", "cde" }, "ab", true)]
        [InlineData(new [] { "abc", "cde" }, "cde", false)]
        [InlineData(new [] { "abc", "cde" }, "efg", false)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void CompareTo_GT_Any_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test.CompareTo(toFind) > 0;
            Assert.Equal(expected, actual);
        }

        [InlineData(new [] { "abc", "cde" }, "abc", false)]
        [InlineData(new [] { "abc", "cde" }, "ab", false)]
        [InlineData(new [] { "abc", "cde" }, "cde", true)]
        [InlineData(new [] { "abc", "cde" }, "efg", true)]
        [InlineData(new string[] { }, "efg", false)]
        [Theory]
        public void CompareTo_LT_Any_True(string[] input, string toFind, bool expected) {
            var test = new LDAPStringList(input);
            var actual = test.CompareTo(toFind) < 0;
            Assert.Equal(expected, actual);
        }
    }
}