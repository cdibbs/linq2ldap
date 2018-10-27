using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Linq2Ldap.FilterParser {
    public class Lexer: ILexer {
        public IEnumerable<Token> Lex(string input) {
            int i = 0, prevTokEnd = 0;
            Token nextTok = null, ucToken;
            while (i < input.Length) {
                switch((nextTok = GetNextToken(input, i, nextTok))?.Text) {
                    case Tokens.EscapedEscape:
                        i = i + 2;
                        break;
                    case Tokens.Escape:
                    case null:
                        i = i + 1;
                        break;
                    default:
                        if (null != (ucToken = GetUserCharsToken(input, i, prevTokEnd))) {
                            yield return ucToken;
                        }

                        i = i + nextTok.Text.Length;
                        prevTokEnd = i;
                        yield return nextTok;
                        break;
                }
            }

            if (null != (ucToken = GetUserCharsToken(input, i, prevTokEnd))) {
                yield return ucToken;
            }
        }

        protected Token GetUserCharsToken(string input, int i, int prevTokEnd) {
            if (i > prevTokEnd) {
                var raw = input.Substring(prevTokEnd, i - prevTokEnd);
                raw = UnescapeAndTrim(raw);
                if (raw.Length > 0) {
                    return new Token(raw, i);
                }
            }

            return null;
        }

        protected Token GetNextToken(string input, int i, Token curToken) {
            if (Tokens.Escape == curToken?.Text) {
                return null;
            }

            Match m;
            foreach (var sregex in Tokens.Lookup.Keys)
            {
                if ((m = new Regex(sregex).Match(input, i)).Success
                    && m.Index == i && m.Length > 0)
                {
                    return new Token(m.Value, i, true, Tokens.Lookup[sregex]);
                }
            }

            return null;
        }

        protected string UnescapeAndTrim(string raw) {
            raw = string.Join("", Regex.Split(raw, @"^(?<!(?<!\\)\\) +"));
            raw = string.Join("", Regex.Split(raw, @"(?<!(?<!\\)\\) +$"));
            return string.Join("", Regex.Split(raw, @"(?<!(?<!\\)\\)\\"));
        }
    }
}