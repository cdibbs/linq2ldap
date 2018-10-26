namespace Linq2Ldap.FilterCompiler.Parser {
    public class Token {
        public Token(string text, int start, bool isDef = false) {
            Text = text;
            StartPos = start;
            EndPos = start + text.Length;
            IsDefaultToken = isDef;
        }
        public string Text { get; set; }
        public int StartPos { get; set; }
        public int EndPos { get; set; }
        public bool IsDefaultToken { get; set; }

        public override string ToString() => Text;
    }
}