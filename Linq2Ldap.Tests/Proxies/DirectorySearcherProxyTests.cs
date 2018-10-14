using System.DirectoryServices;
using Linq2Ldap.Proxies;
using Xunit;

namespace Linq2Ldap.Tests.Proxies {
    public class DirectorySearcherProxyTests {

        [Fact]
        public void Constructs_PassesBaseEntry() {
            var entry = new DirectoryEntry();
            var p = new DirectorySearcherProxy(entry);
            Assert.Equal(entry, p.SearchRoot);
        }
    }
}