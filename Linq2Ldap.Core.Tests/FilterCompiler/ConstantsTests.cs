using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core.Models;
using Moq;
using Xunit;

namespace Linq2Ldap.Core.Tests.FilterCompiler
{
    public class ConstantsTests
    {
        public LdapFilterCompiler FilterCompiler;
        public ConstantsTests()
        {
            FilterCompiler = new LdapFilterCompiler();
        }

        [Fact]
        public void _MemberToString_ThrowsForNonBoolNonProp()
        {
            var str = "stringy";
            Expression<Func<TestLdapModel, object>> expr = (TestLdapModel m) => str;
            var cc = new CompilerCore();
            Assert.Throws<NotImplementedException>(() =>
                cc._MemberToString(expr.Body as MemberExpression, expr.Parameters));
        }


        [Fact]
        public void InlineConstant_CompilesToValue()
        {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => u.SamAccountName == "something";
            var result = FilterCompiler.Compile(expr);
            Assert.Equal("(samaccountname=something)", result);
        }


        [Fact]
        public void Constant_CompilesToValue()
        {
            const string c = "something";
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => u.SamAccountName == c;
            var result = FilterCompiler.Compile(expr);
            Assert.Equal("(samaccountname=something)", result);
        }

        [InlineData(true, "(&)")]
        [InlineData(false, "(|)")]
        [Theory]
        public void BooleanMember_Compiles(bool val, string expected) {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => val;
            var result = FilterCompiler.Compile(expr);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConstantTrue_Canonical() {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => true;
            var result = FilterCompiler.Compile(expr);
            Assert.Equal("(&)", result);

            expr = (TestLdapModel u) => true && u["something"] == "one";
            result = FilterCompiler.Compile(expr);
            Assert.Equal("(&(&)(something=one))", result);
        }

        [Fact]
        public void ConstantFalse_Canonical() {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => false;
            var result = FilterCompiler.Compile(expr);
            Assert.Equal("(|)", result);

            expr = (TestLdapModel u) => false && u["something"] == "one";
            result = FilterCompiler.Compile(expr);
            Assert.Equal("(&(|)(something=one))", result);
        }

    }
}
