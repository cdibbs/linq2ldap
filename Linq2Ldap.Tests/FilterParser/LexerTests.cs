using System.Linq;
using Linq2Ldap.FilterParser;
using Xunit;
using Newtonsoft.Json;
using System;

namespace Linq2Ldap.Tests.FilterParser {
    public class LexerTests {
        public Lexer Lexer;
        public LexerTests() {
            Lexer = new Lexer();
        }

        [InlineData(
            "((one=two)(two~=three))",
            new [] { "(", "(", "one", "=", "two", ")", "(", "two", "~=", "three", ")", ")"})]
        [InlineData("()", new [] { "(", ")" })]
        [InlineData("", new string[0])]
        [InlineData(@"(&(a=b)(b=c))",
            new [] { "(", "&", "(", "a", "=", "b", ")", "(", "b", "=", "c", ")", ")" })]
        [InlineData(@"(|ab))",
            new [] { "(", "|", "ab", ")", ")"}) /* invalid parse, valid lex */]
        [Theory]
        public void Lex_BuildsCorrectTokenArray(string input, string[] expected) {
            var actual = Lexer.Lex(input);
            var actualStrs = actual.Select(t => t.Text).ToArray();
            Assert.Equal(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(actualStrs));
        }

        [InlineData(@" ( a = b )  ", new [] { "(", "a", "=", "b", ")" })]
        [InlineData(@" ( a =\ b\ )  ", new [] { "(", "a", "=", " b ", ")" })]
        [InlineData(@"(\\\&var=b)",
            new [] { "(", @"\&var", "=", "b", ")" })]
        [InlineData(@"\\ asdf", new [] { @"\ asdf" })]
        [Theory]
        public void Lex_DealsWithEscapesAndSpacesCorrectly(string input, string[] expected) {
            var actual = Lexer.Lex(input);
            var actualStrs = actual.Select(t => t.Text).ToArray();
            Assert.Equal(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(actualStrs));
        }
    }
}