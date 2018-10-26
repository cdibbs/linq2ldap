using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Linq2Ldap.FilterCompiler.Parser {
    public class Lexer {
        public static string[] AllTokens = new string[] {
            Tokens.LeftParen,
            Tokens.RightParen,
            Tokens.Approx,
            Tokens.LTE,
            Tokens.GTE,
            Tokens.Present,
            Tokens.Equal,
            Tokens.And,
            Tokens.Or,
            Tokens.Not,
            Tokens.EscapedEscape,
            Tokens.Escape
        };

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

            foreach (var tok in AllTokens)
            {
                if (i + tok.Length <= input.Length
                    && input.IndexOf(tok, i) == i)
                {
                    return new Token(tok, i, true);
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