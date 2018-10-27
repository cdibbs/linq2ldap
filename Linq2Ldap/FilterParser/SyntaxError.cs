using System;

namespace Linq2Ldap.FilterParser {
    public class SyntaxException: Exception {
        public SyntaxException(string msg, int start, int end): base($"{msg}\nStart: {start}, End: {end}") {
        }
    }
}