namespace Linq2Ldap.FilterCompiler.Parser {
    public static class Tokens {
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