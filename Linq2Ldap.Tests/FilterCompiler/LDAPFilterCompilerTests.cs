using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.FilterCompiler;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class LDAPFilterCompilerTests
    {
        private LDAPFilterCompiler FilterCompiler;
        private CompilerCore Core;
        public LDAPFilterCompilerTests()
        {
            FilterCompiler = new LDAPFilterCompiler();
            Core = new CompilerCore();
        }

        [Fact]
        public void _MemberToString_NonDataSourceModel_Throws()
        {
            var scopedModel = new { Weird = 123 };
            Expression<Func<User, bool>> expr1 = (User u) => scopedModel.Weird == 123;
            var member = ((BinaryExpression)expr1.Body).Left;
            Action lambda = () => Core._MemberToString(member as MemberExpression, expr1.Parameters);
            Assert.Throws<NotImplementedException>(lambda);
        }

        [Fact]
        public void _MemberToString_DataSourceModel_SerializesByColumnAttrWhenAvailable()
        {
            Expression<Func<User, bool>> expr1 = (User u) => u.SamAccountName == "something";
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
            Func<string> testfn = () => "samaccountname"; // user can always invoke prior to express
            Expression<Func<User, bool>> expr1 = (User u) => u.Properties[testfn()] == "123";
            Assert.Throws<NotImplementedException>(() => FilterCompiler.CompileFromLinq(expr1));
        }

        [Fact]
        public void CompileFromLinq_AndAlsoWithSubExpr_GeneratesValidLDAPFilterString()
        {
            Expression<Func<User, bool>> e
                = (User u) => u.SamAccountName.Contains("test") && u.CommonName == "123";
            var result = FilterCompiler.CompileFromLinq(e);
            Assert.Equal("(&(samaccountname=*test*)(cn=123))", result);
        }

        [Fact]
        public void CompileFromLinq_StringCompare_NonConstParam()
        {
            Func<string> testfn = () => "samaccountname"; // user can always invoke prior to express
            Expression<Func<User, bool>> e
                = (User u) => u.SamAccountName.Contains(testfn());
            Assert.Throws<NotImplementedException>(() => FilterCompiler.CompileFromLinq(e));
        }

        [Fact]
        public void CompileFromLinq_UnsupportedExpressionType_Throws()
        {
            int a = 3, b = 4;
            Expression<Func<User, bool>> e
                = (User u) => a + b == 7;
            Assert.Throws<NotImplementedException>(() => FilterCompiler.CompileFromLinq(e));
        }

        [Fact]
        public void _EvalExpr_ThrowsWhenNotRecognizedType()
        {
            Expression<Func<User, bool>> e = (User u) => u.SamAccountName.Contains("asdf");
            Assert.Throws<NotImplementedException>(() => Core.EvalExpr(e.Body, e.Parameters));
        }

        [Fact]
        public void _EvalExpr_EscapesValues() {
            Expression<Func<User, string>> e = (User u) => @"must*escape\this";
            var result = Core.EvalExpr(e.Body, e.Parameters);
            Assert.Equal(@"must\*escape\\this", result);
        }
    }

    public class TestUser : User
    {
        public string UserlandProp { get; set; }
    }
}
