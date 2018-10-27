using Xunit;
using Linq2Ldap.FilterParser;
using Linq2Ldap.Models;
using System.Collections.Generic;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Tests.FilterParser {
    public class ParserTests {
        public Parser Parser;
        public ParserTests() {
            Parser = new Parser();
        }

        [Fact]
        public void Parse_Integration_ThrowsOnMissingParens() {
            Assert.Throws<SyntaxException>(() => Parser.Parse<Entry>("a=b"));
        }

        [InlineData("(a=b)", true)]
        [InlineData("(a=c)", false)]
        [InlineData("(!(a=b))", false)]
        [InlineData("(!(a=c))", true)]
        [InlineData("(c=d)", true)]
        [InlineData("(c<=e)", true)]
        [InlineData("(c>=e)", false)]
        [InlineData("(c>=c)", true)]
        [InlineData("(e=31*)", true)]
        [InlineData("(e=*14)", true)]
        [InlineData("(e=*1*)", true)]
        [InlineData("(e=*2*)", false)]
        [InlineData("(e=21*)", false)]
        [InlineData("(e=*15)", false)]
        [Theory]
        public void Parse_Integration_BasicBooleans(string input, bool expected) {
            var expr = Parser.Parse<Entry>(input);
            var dict = new Dictionary<string, ResultPropertyValueCollectionProxy>() {
                { "a", new ResultPropertyValueCollectionProxy(new List<object>() { "b" }) },
                { "c", new ResultPropertyValueCollectionProxy(new List<object>() { "d" }) },
                { "e", new ResultPropertyValueCollectionProxy(new List<object>() { "314" }) }
            };
            var entry = new Entry() { Properties = new Linq2Ldap.Proxies.ResultPropertyCollectionProxy(dict) };
            Assert.Equal(expected, expr.Compile()(entry));

        }
    }
}