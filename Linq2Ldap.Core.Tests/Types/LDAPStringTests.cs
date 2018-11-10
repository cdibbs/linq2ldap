using System.Collections.Generic;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.Core.Types;
using Xunit;

namespace Linq2Ldap.Core.Tests.Types {
    public class LdapStringTests {
        [Fact]
        public void ImplicitToString_ReturnsOriginal() {
            var testStr = "something";
            var test = new LdapString(new PropertyValueCollection(new List<object>(){ testStr }));
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