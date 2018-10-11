using Xunit;
using Linq2Ldap.ExtensionMethods;

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
        [Theory]
        public void Matches_MatchesValidRFC1960RightHandStrings(string input, string pattern, bool expectedResult) {
            var actual = input.Matches(pattern);
            Assert.Equal(expectedResult, actual);
        }
    }
}