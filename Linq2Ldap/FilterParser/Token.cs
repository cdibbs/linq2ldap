namespace Linq2Ldap.FilterParser {
    public class Token {
        public Token(string text, int start, bool isDef = false, string matchedToken = null) {
            Text = text;
            StartPos = start;
            EndPos = start + text.Length;
            IsDefinedSymbol = isDef;
            MatchedToken = matchedToken;
        }

        public string MatchedToken { get; set; }
        public string Text { get; set; }
        public int StartPos { get; set; }
        public int EndPos { get; set; }

        /// <summary>
        /// Indicates whether this is one of the defined symbols.
        /// </summary>
        /// <value>True, if in the set of defined symbols. False if user text.</value>
        public bool IsDefinedSymbol { get; set; }

        public override string ToString() => Text;
    }
}