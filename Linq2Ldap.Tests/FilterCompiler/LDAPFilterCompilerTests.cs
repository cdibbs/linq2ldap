using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.FilterCompiler;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class LdapFilterCompilerTests
    {
        private LdapFilterCompiler FilterCompiler;
        private CompilerCore Core;
        public LdapFilterCompilerTests()
        {
            FilterCompiler = new LdapFilterCompiler();
            Core = new CompilerCore();
        }

        [Fact]
        public void _MemberToString_NonDataSourceModel_Throws()
        {
            var scopedModel = new { Weird = 123 };
            Expression<Func<TestLdapModel, bool>> expr1 = (TestLdapModel u) => scopedModel.Weird == 123;
            var member = ((BinaryExpression)expr1.Body).Left;
            Action lambda = () => Core._MemberToString(member as MemberExpression, expr1.Parameters);
            Assert.Throws<NotImplementedException>(lambda);
        }

        [Fact]
        public void CompileFromLinq_EscapesIndexerNames() {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel e) => e["one "] == "123";
            var filter = Core.ExpressionToString(expr.Body, expr.Parameters);
            Assert.Equal(@"(one\ =123)", filter);
        }

        [Fact]
        public void CompileFromLinq_EscapesPropertyNames() {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel e) => e.WeirdName == "123";
            var filter = Core.ExpressionToString(expr.Body, expr.Parameters);
            Assert.Equal(@"(\ we ird\ \ =123)", filter);
        }

        [Fact]
        public void _MemberToString_DataSourceModel_SerializesByColumnAttrWhenAvailable()
        {
            Expression<Func<TestLdapModel, bool>> expr1 = (TestLdapModel u) => u.SamAccountName == "something";
            var member = ((BinaryExpression)expr1.Body).Left;
            var result = Core._MemberToString(member as MemberExpression, expr1.Parameters);
            Assert.Equal("samaccountname", result);

            Expression<Func<TestUser, bool>> expr2 = (TestUser u) => u.UserlandProp == "something";
            member = ((BinaryExpression)expr2.Body).Left;
            result = Core._MemberToString(member as MemberExpression, expr2.Parameters);
            Assert.Equal("UserlandProp", result);
        }

        [Fact]
        public void CompileFromLinq_NonConstPDictKey_Throws() // KIS: why maintain unnec. complexity?
        {
            Func<string> testfn = () => "samaccountname"; // TestLdapModel can always invoke prior to express
            Expression<Func<TestLdapModel, bool>> expr1 = (TestLdapModel u) => u.Properties[testfn()] == "123";
            Assert.Throws<NotImplementedException>(() => FilterCompiler.Compile(expr1));
        }

        [Fact]
        public void CompileFromLinq_AndAlsoWithSubExpr_GeneratesValidLDAPFilterString()
        {
            Expression<Func<TestLdapModel, bool>> e
                = (TestLdapModel u) => u.SamAccountName.Contains("test") && u.CommonName == "123";
            var result = FilterCompiler.Compile(e);
            Assert.Equal("(&(samaccountname=*test*)(cn=123))", result);
        }

        [Fact]
        public void CompileFromLinq_StringCompare_NonConstParam()
        {
            Func<string> testfn = () => "samaccountname"; // TestLdapModel can always invoke prior to express
            Expression<Func<TestLdapModel, bool>> e
                = (TestLdapModel u) => u.SamAccountName.Contains(testfn());
            Assert.Throws<NotImplementedException>(() => FilterCompiler.Compile(e));
        }

        [Fact]
        public void CompileFromLinq_UnsupportedExpressionType_Throws()
        {
            int a = 3, b = 4;
            Expression<Func<TestLdapModel, bool>> e
                = (TestLdapModel u) => a + b == 7;
            Assert.Throws<NotImplementedException>(() => FilterCompiler.Compile(e));
        }

        [Fact]
        public void _EvalExpr_ThrowsWhenNotRecognizedType()
        {
            Expression<Func<TestLdapModel, bool>> e = (TestLdapModel u) => u.SamAccountName.Contains("asdf");
            Assert.Throws<NotImplementedException>(() => Core.EvalExpr(e.Body, e.Parameters));
        }

        [Fact]
        public void _EvalExpr_EscapesValues() {
            Expression<Func<TestLdapModel, string>> e = (TestLdapModel u) => @"must*escape\this";
            var result = Core.EvalExpr(e.Body, e.Parameters);
            Assert.Equal(@"must\*escape\\this", result);
        }
    }

    public class TestUser : TestLdapModel
    {
        public string UserlandProp { get; set; }
    }
}
