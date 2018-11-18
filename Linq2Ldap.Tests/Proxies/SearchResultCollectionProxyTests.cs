using System.DirectoryServices;
using Linq2Ldap.Proxies;
using Xunit;
using Linq2Ldap.TestCommon;
using System.Collections.Generic;
using System.Collections;

namespace Linq2Ldap.Tests.Proxies {
    public class SearchResultCollectionProxyTests {

        [WindowsOnlyFact]
        public void Enumerates() {
            var p = new SearchResultCollectionProxy(
                new List<SearchResultProxy>() { new SearchResultProxy() });
            foreach (var e in (p as IEnumerable)) {
                Assert.IsType<SearchResultProxy>(e);
            }
        }

        [WindowsOnlyFact]
        public void ImplicitlyConverts() {
            SearchResultCollection c = null;
            SearchResultCollectionProxy p = c;
        }
    }
}