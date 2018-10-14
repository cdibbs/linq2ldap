using Xunit;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.Types;

namespace Linq2Ldap.Tests.ExtensionMethods {
    public class StringExtensionMethodsTests {

        [InlineData("", "*", true)]
        [InlineData("asdfaf", "*", true)]
        [InlineData("***", "*", true)]
        [InlineData("asdf", "as*", true)]
        [InlineData("university of iowa", "univ*of*iowa", true)]
        [InlineData("university of iowa", "univ*of*il", false)]
        [InlineData("university of iowa", "univ*iowa", true)]
        [InlineData("university of iowa", "univ*", true)]
        [InlineData("university of iowa", "*iowa", true)]
        [InlineData("university of iowa", "univ*for*iowa", false)]
        [InlineData(null, "univ*for*iowa", false)]
        [InlineData(null, "*", false)]
        [Theory]
        public void Matches_MatchesValidRFC1960RightHandStrings(string input, string pattern, bool expectedResult) {
            var actual = input.Matches(pattern);
            Assert.Equal(expectedResult, actual);
        }

        [InlineData(new [] { "university of iowa" }, "univ*of*iowa", true)]
        [InlineData(new [] { "university of iowa" }, "*", true)]
        [InlineData(new string[] { }, "*", true)]
        [InlineData(null, "*", false)]
        [Theory]
        public void Matches_LDAPStringList_MatchesValidRFC1960RightHandStrings(string[] input, string pattern, bool expectedResult) {
            LDAPStringList i = input == null ? (LDAPStringList)null : input;
            Assert.Equal(expectedResult, i.Matches(pattern));
        }

        [InlineData("university of IOWA", "*iowa", true)]
        [InlineData("university of IOWA", "univ*for*iowa", false)]
        [Theory]
        public void Approx_MatchesInvariant(string input, string pattern, bool expectedResult) {
            var actual = input.Approx(pattern);
            Assert.Equal(expectedResult, actual);
        }

        [InlineData(new [] { "university of IOWA" }, "*iowa", true)]
        [InlineData(new [] { "university of IOWA" }, "univ*for*iowa", false)]
        [InlineData(new string[] { }, "*", true)]
        [InlineData(null, "*", false)]
        [Theory]
        public void Approx_LDAPStringList_MatchesInvariant(string[] input, string pattern, bool expectedResult) {
            LDAPStringList i = input == null ? (LDAPStringList)null : input;
            var actual = i.Approx(pattern);
            Assert.Equal(expectedResult, actual);
        }
    }
}