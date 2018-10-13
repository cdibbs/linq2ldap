using Linq2Ldap.Types;
using Xunit;

namespace Linq2Ldap.Tests.Types {
    public class LDAPStringTests {
        [Fact]
        public void ImplicitToString_ReturnsOriginal() {
            var testStr = "something";
            var test = new LDAPString(testStr);
            string result = test;
            Assert.Equal(testStr, result);
        }

        [Fact]
        public void ImplicitToLDAPString_ReturnsWrapped() {
            string testStr = "something";
            LDAPString s = testStr;
            Assert.Equal(testStr, s.ToString());
        }
    }
}