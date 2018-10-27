using System.Collections.Generic;

namespace Linq2Ldap.FilterParser {
    public interface ILexer
    {
        IEnumerable<Token> Lex(string input);
    }
}