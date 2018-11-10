using System.Collections.Generic;
using Linq2Ldap.Core.Proxies;
using Xunit;

namespace Linq2Ldap.Core.Tests.Proxies {
    public class ResultPropertyCollectionProxyTests {

        [Fact]
        public void Contains_ReturnsMockResults() {
            var m = new Dictionary<string, Core.Proxies.PropertyValueCollection>() {
                { "a key", new Core.Proxies.PropertyValueCollection(new List<object>()) }
            };
            var p = new DirectoryEntryPropertyCollection(m);
            Assert.True(p.ContainsKey("a key"));
            Assert.False(p.ContainsKey("another key"));
        }

        [Fact]
        public void Count_ReturnsMockResults() {
            var m = new Dictionary<string, Core.Proxies.PropertyValueCollection>() {
                { "a key", new Core.Proxies.PropertyValueCollection(new List<object>()) },
                { "2 key", new Core.Proxies.PropertyValueCollection(new List<object>()) },
                { "3 key", new Core.Proxies.PropertyValueCollection(new List<object>()) }
            };
            var p = new DirectoryEntryPropertyCollection(m);
            Assert.Equal(3, p.Count);
        }
    }
}