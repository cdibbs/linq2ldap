using System.Text.RegularExpressions;

namespace Linq2Ldap.ExtensionMethods {
    public static class StringExtensions {
        public static bool Matches(this string source, string pattern) {
            var pieces = Regex.Split(pattern, @"(?<!\\)\*"); // non-escaped asterisk
            if (pieces.Length == 1 && pieces[0] == "*") {
                return true;
            }

            int i = 0, p = 0;
            for (; i < pieces.Length && p != -1; p = source.IndexOf(pieces[i++], p));
            return i == pieces.Length && p != -1;
        }

        public static bool Approx(this string source, string pattern) {
            return Matches(source.ToLowerInvariant(), pattern.ToLowerInvariant());
        }
    }
}