using System.Collections.Generic;

namespace Linq2Ldap.FilterParser {
    public static class Tokens {
        public static readonly Dictionary<string, string> Lookup
            = new Dictionary<string, string>() {
                //  (?<= non-capturing look-behind
                //      \G to match only where previous match left off
                //        (\\\\)*) and only when an even number of backslashes (when operator unescaped).
                { @"(?<=\G(\\\\)*)\(",               LeftParen },
                { @"(?<=\G(\\\\)*)\)",               RightParen },
                { @"(?<=\G(\\\\)*)\&",               And },
                { @"(?<=\G(\\\\)*)\|",               Or },
                { @"(?<=\G(\\\\)*)\!",               Not },
                { @"(?<=\G(\\\\)*)=\*(?=\s*[\)])", Present },
                { @"(?<=\G(\\\\)*)=",               Equal },
                { @"(?<=\G(\\\\)*)>=",              GTE },
                { @"(?<=\G(\\\\)*)<=",              LTE },
                { @"(?<=\G(\\\\)*)\*",              Star },
                { @"(?<=\G(\\\\)*)~=",              Approx },
                { @"(?<=\G(\\\\)*)\\\\",            EscapedEscape },
                { @"(?<=\G(\\\\)*)\\",              Escape },
            };

        public const string LeftParen = "(";
        public const string RightParen = ")";
        public const string And = "&";
        public const string Or = "|";
        public const string Not = "!";
        public const string Present = "=*";
        public const string GTE = ">=";
        public const string LTE = "<=";
        public const string Equal = "=";
        public const string Star = "*";
        public const string Approx = "~=";
        public const string EscapedEscape = @"\\";
        public const string Escape = @"\";
    }
}