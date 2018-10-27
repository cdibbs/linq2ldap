using System.Collections.Generic;

namespace Linq2Ldap.FilterParser {
    public static class Tokens {
        public static readonly Dictionary<string, string> Lookup
            = new Dictionary<string, string>() {
                { @"(?<!(?<!\\)\\)*\(",               LeftParen },
                { @"(?<!(?<!\\)\\)*\)",               RightParen },
                { @"(?<!(?<!\\)\\)*\&",               And },
                { @"(?<!(?<!\\)\\)*\|",               Or },
                { @"(?<!(?<!\\)\\)*\!",               Not },
                { @"(?<!(?<!\\)\\)*=\*(?=\s*[\)])", Present },
                { @"(?<!(?<!\\)\\)*=",               Equal },
                { @"(?<!(?<!\\)\\)*>=",              GTE },
                { @"(?<!(?<!\\)\\)*<=",              LTE },
                { @"(?<!(?<!\\)\\)*\*",              Star },
                { @"(?<!(?<!\\)\\)*~=",              Approx },
                { @"(?<!(?<!\\)\\)*\\\\",            EscapedEscape },
                { @"(?<!(?<!\\)\\)*\\",              Escape },
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