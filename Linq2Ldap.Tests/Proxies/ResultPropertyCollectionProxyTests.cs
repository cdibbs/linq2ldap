using System.Collections.Generic;
using System.DirectoryServices;
using Linq2Ldap.Proxies;
using Xunit;

namespace Linq2Ldap.Tests.Proxies {
    public class ResultPropertyCollectionProxyTests {

        [Fact]
        public void Contains_ReturnsMockResults() {
            var m = new Dictionary<string, ResultPropertyValueCollectionProxy>() {
                { "a key", new ResultPropertyValueCollectionProxy(new List<object>()) }
            };
            var p = new ResultPropertyCollectionProxy(m);
            Assert.True(p.Contains("a key"));
            Assert.False(p.Contains("another key"));
        }

        [Fact]
        public void Count_ReturnsMockResults() {
            var m = new Dictionary<string, ResultPropertyValueCollectionProxy>() {
                { "a key", new ResultPropertyValueCollectionProxy(new List<object>()) },
                { "2 key", new ResultPropertyValueCollectionProxy(new List<object>()) },
                { "3 key", new ResultPropertyValueCollectionProxy(new List<object>()) }
            };
            var p = new ResultPropertyCollectionProxy(m);
            Assert.Equal(3, p.Count);
        }
    }
}