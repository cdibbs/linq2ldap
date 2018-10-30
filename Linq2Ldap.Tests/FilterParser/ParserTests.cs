using Xunit;
using Linq2Ldap.FilterParser;
using Linq2Ldap.Models;
using System.Collections.Generic;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Tests.FilterParser {
    public class ParserTests {
        public LdapFilterParser Parser;
        public ParserTests() {
            Parser = new LdapFilterParser();
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
        [InlineData("(&(a=b)(c=d))", true)]
        [InlineData("(&(a=b)(!(c=d)))", false)]
        [InlineData("(|(a=b)(!(c=d)))", true)]
        [InlineData("(&(a=b)(!(c=d)))", false)]
        [InlineData("(&(a=b)(c=d)(e>=31)))", true)]
        [InlineData("(&(a=b)(c=d)(e>=315)))", false)]
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

        [InlineData("(a=*)", true)]
        [InlineData("(multiv=*)", true)]
        [InlineData("(f=*)", false)]
        [InlineData("(emptylist=*)", true)]
        [Theory]
        public void Parse_Integration_ExistenceChecks(string input, bool expected) {
            var expr = Parser.Parse<Entry>(input);
            var dict = new Dictionary<string, ResultPropertyValueCollectionProxy>() {
                { "a", new ResultPropertyValueCollectionProxy(new List<object>() { "b" }) },
                { "multiv", new ResultPropertyValueCollectionProxy(new List<object>() { "d", "e" }) },
                { "emptylist", new ResultPropertyValueCollectionProxy(new List<object>() { }) }
            };
            var entry = new Entry() { Properties = new Linq2Ldap.Proxies.ResultPropertyCollectionProxy(dict) };
            Assert.Equal(expected, expr.Compile()(entry));
        }

        [InlineData("(a~=B)", true)]
        [InlineData("(a~=b)", true)]
        [InlineData("(a~=C)", false)]
        [InlineData("(a~=c)", false)]
        [InlineData("(c~=E)", true)]
        [Theory]
        public void Parse_Integration_ApproxChecks(string input, bool expected) {
            var expr = Parser.Parse<Entry>(input);
            var dict = new Dictionary<string, ResultPropertyValueCollectionProxy>() {
                { "a", new ResultPropertyValueCollectionProxy(new List<object>() { "b" }) },
                { "c", new ResultPropertyValueCollectionProxy(new List<object>() { "d", "e" }) },
            };
            var entry = new Entry() { Properties = new Linq2Ldap.Proxies.ResultPropertyCollectionProxy(dict) };
            Assert.Equal(expected, expr.Compile()(entry));
        }

        [InlineData(@"(a\==b)", true)]
        [InlineData(@"(a=b)", false)]
        [InlineData(@"(a=*)", false)]
        [InlineData(@"(a\==*)", true)]
        [InlineData(@"(a=*)", false)]
        [InlineData(@"(c\\=*)", true)]
        [InlineData(@"(c\==*)", false)]
        [Theory]
        public void Parse_Integration_EscapeChecks(string input, bool expected) {
            var expr = Parser.Parse<Entry>(input);
            var dict = new Dictionary<string, ResultPropertyValueCollectionProxy>() {
                { @"a=", new ResultPropertyValueCollectionProxy(new List<object>() { "b" }) },
                { @"c\", new ResultPropertyValueCollectionProxy(new List<object>() { "d", "e" }) },
            };
            var entry = new Entry() { Properties = new Linq2Ldap.Proxies.ResultPropertyCollectionProxy(dict) };
            Assert.Equal(expected, expr.Compile()(entry));
        }
    }
}