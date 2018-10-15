using System.DirectoryServices;
using Linq2Ldap.Proxies;
using Xunit;
using Linq2Ldap.TestUtil;
using System.Collections.Generic;

namespace Linq2Ldap.Tests.Proxies
{
    public class SearchResultProxyTests
    {
        [WindowsOnlyFact]
        public void ImplicitlyConverts() {
            SearchResult sr = null;
            SearchResultProxy p = sr;
        }

        [WindowsOnlyFact]
        public void Constructs() {
            var props = new ResultPropertyCollectionProxy(new Dictionary<string, ResultPropertyValueCollectionProxy>());
            var path = "something";
            var p = new SearchResultProxy(null, props, path);
            Assert.Equal(path, p.Path);
            Assert.Equal(props, p.Properties);
        }
    }
}