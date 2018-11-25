using System.DirectoryServices;
using Linq2Ldap.Proxies;
using Xunit;
using Linq2Ldap.TestCommon;
using System.Collections.Generic;
using Linq2Ldap.Core.Proxies;
using System.Runtime.Serialization;

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
            var props = new EntryAttributeDictionary(new Dictionary<string, Core.Proxies.AttributeValueList>());
            var path = "something";
            var p = new SearchResultProxy(null, props, path);
            Assert.Equal(path, p.Path);
            Assert.Equal(props, p.Properties);
        }
    }
}