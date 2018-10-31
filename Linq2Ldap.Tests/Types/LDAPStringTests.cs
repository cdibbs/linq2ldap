using System.Collections.Generic;
using Linq2Ldap.Proxies;
using Linq2Ldap.Types;
using Xunit;

namespace Linq2Ldap.Tests.Types {
    public class LdapStringTests {
        [Fact]
        public void ImplicitToString_ReturnsOriginal() {
            var testStr = "something";
            var test = new LdapString(new ResultPropertyValueCollectionProxy(new List<object>(){ testStr }));
            string result = test;
            Assert.Equal(testStr, result);
        }

        [Fact]
        public void ImplicitToLdapString_ReturnsWrapped() {
            string testStr = "something";
            LdapString s = testStr;
            Assert.Equal(testStr, s.ToString());
        }
    }
}