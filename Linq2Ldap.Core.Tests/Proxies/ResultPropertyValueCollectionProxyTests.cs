using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Linq2Ldap.Core.Tests.Proxies
{
    public class ResultPropertyValueCollectionProxyTests {

        [Fact]
        public void Constructor_ThrowsOnNull() {
            Assert.Throws<ArgumentNullException>(() => new Core.Proxies.PropertyValueCollection((List<object>)null));
        }

        [Fact]
        public void Enumerator_Enumerates() {
            var m = new List<object>() { "a", "b", "c" };
            var p = new Core.Proxies.PropertyValueCollection(m);
            var i = 0;
            foreach (var e in p) {
                Assert.Equal(e, m[i++]);
            }
            Assert.Equal(3, i);
        }

        [Fact]
        public void GenericEnumerator_SameAsNonGeneric() {
            var m = new List<object>() { "a", "b", "c" };
            var p = new Core.Proxies.PropertyValueCollection(m);
            var i = 0;
            foreach (var e in (p as IEnumerable)) {
                Assert.Equal(e, m[i++]);
            }
        }

        [InlineData(new string[] { }, new int[] { })]
        [InlineData(new string[] { "a", "b", "c" }, new int[] { -1, 0, 1 })]
        [InlineData(new string[] { "b" }, new int[] { 0 })]
        [Theory]
        public void CompareTo_ReturnsListOfResults(string[] s, int[] expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p.CompareTo("b"));
        }

        [InlineData(new string[] { }, "a", false)]
        [InlineData(new string[] { "a", "b", "c" }, "b", true)]
        [InlineData(new string[] { "a", "b", "c" }, "c", true)]
        [InlineData(new string[] { "b" }, "c", false)]
        [Theory]
        public void Equals_ReturnsTrueWhenEqual(string[] s, string test, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p.Equals(test));
        }

        [InlineData(new string[] { }, "a", true)]
        [InlineData(new string[] { "a", "b", "c" }, "b", false)]
        [InlineData(new string[] { "a", "b", "c" }, "c", false)]
        [InlineData(new string[] { "b" }, "c", true)]
        [Theory]
        public void NotEquals_ReturnsTrueWhenNotEqual(string[] s, string test, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p != test);
        }      

        [InlineData(new string[] { }, "a", false)]
        [InlineData(new string[] { "a", "b", "c" }, "b", true)]
        [InlineData(new string[] { "a", "b", "c" }, "c", true)]
        [InlineData(new string[] { "a", "b", "c" }, "a", false)]
        [InlineData(new string[] { "a", "b", "c" }, "0", false)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "doe", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "abacus", false)]
        [InlineData(new string[] { "b" }, "c", true)]
        [Theory]
        public void LessThan_ReturnsTrueWhenLT(string[] s, string test, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p < test);
        }

        [Fact]
        public void LessThan_Null_Throws() {
            Core.Proxies.PropertyValueCollection p2 = null;
            Assert.Throws<ArgumentException>(() => p2 < "a");
            Assert.Throws<ArgumentException>(() => p2 < null);
        }

        [InlineData(new string[] { }, "a", false)]
        [InlineData(new string[] { "a", "b", "c" }, "b", true)]
        [InlineData(new string[] { "a", "b", "c" }, "c", false)]
        [InlineData(new string[] { "a", "b", "c" }, "a", true)]
        [InlineData(new string[] { "a", "b", "c" }, "0", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "arbor", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "canada", false)]
        [InlineData(new string[] { "b" }, "c", false)]
        [Theory]
        public void GreaterThan_ReturnsTrueWhenGT(string[] s, string test, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p > test);
        }

        [Fact]
        public void GreaterThan_Null_Throws() {
            Core.Proxies.PropertyValueCollection p2 = null;
            Assert.Throws<ArgumentException>(() => p2 > "a");
            Assert.Throws<ArgumentException>(() => p2 > null);
        }

        [InlineData(new string[] { }, "a", false)]
        [InlineData(new string[] { "a", "b", "c" }, "b", true)]
        [InlineData(new string[] { "a", "b", "c" }, "c", true)]
        [InlineData(new string[] { "a", "b", "c" }, "a", true)]
        [InlineData(new string[] { "a", "b", "c" }, "0", false)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "doe", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "abra", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "abr", false)]
        [InlineData(new string[] { "b" }, "c", true)]
        [Theory]
        public void LessThanOrEqual_ReturnsTrueWhenLTE(string[] s, string test, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p <= test);
        }

        [Fact]
        public void LessThanOrEqual_Null_Throws() {
            Core.Proxies.PropertyValueCollection p2 = null;
            Assert.Throws<ArgumentException>(() => p2 <= "a");
            Assert.Throws<ArgumentException>(() => p2 <= null);
        }

        [InlineData(new string[] { }, "a", false)]
        [InlineData(new string[] { "a", "b", "c" }, "c", true)]
        [InlineData(new string[] { "a", "b", "c" }, "d", false)]
        [InlineData(new string[] { "a", "b", "c" }, "a", true)]
        [InlineData(new string[] { "a", "b", "c" }, "0", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "doe", false)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "abra", true)]
        [InlineData(new string[] { "abra", "cadabra", "c" }, "abr", true)]
        [InlineData(new string[] { "abra", "cadabra" }, "cadabra", true)]
        [InlineData(new string[] { "b" }, "c", false)]
        [Theory]
        public void GreaterThanOrEqual_ReturnsTrueWhenLTE(string[] s, string test, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p >= test);
        }

        [Fact]
        public void GreaterThanOrEqual_Null_Throws() {
            Core.Proxies.PropertyValueCollection p2 = null;
            Assert.Throws<ArgumentException>(() => p2 >= "a");
            Assert.Throws<ArgumentException>(() => p2 >= null);
        }

        [InlineData(new [] { "abcdef" }, "abc", true)]
        [InlineData(new [] { "abcdef" }, "def", false)]
        [InlineData(new [] { "abcdef", "defghi" }, "def", true)]
        [InlineData(new [] { "abcdef", "defghi" }, "jkl", false)]
        [Theory]
        public void StartsWith_TrueWhen(string[] s, string frag, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p.StartsWith(frag));
        }

        [InlineData(new [] { "abcdef" }, "abc", false)]
        [InlineData(new [] { "abcdef" }, "def", true)]
        [InlineData(new [] { "abcdef", "defghi" }, "def", true)]
        [InlineData(new [] { "abcdef", "defghi" }, "jkl", false)]
        [Theory]
        public void EndsWith_TrueWhen(string[] s, string frag, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p.EndsWith(frag));
        }

        [InlineData(new [] { "abcdef" }, "abc", true)]
        [InlineData(new [] { "abcdef" }, "def", true)]
        [InlineData(new [] { "abcdef" }, "123", false)]
        [InlineData(new [] { "abcdef", "defghi" }, "def", true)]
        [InlineData(new [] { "abcdef", "defghi" }, "jkl", false)]
        [InlineData(new [] { "abcdef", "defghi" }, "fgh", true)]
        [Theory]
        public void Contains_TrueWhen(string[] s, string frag, bool expected) {
            Core.Proxies.PropertyValueCollection p = s;
            Assert.Equal(expected, p.Contains(frag));
        }
    }
}