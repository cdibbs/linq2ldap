using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class ConstantsTests
    {
        public LDAPFilterCompiler FilterCompiler;
        public ConstantsTests()
        {
            FilterCompiler = new LDAPFilterCompiler();
        }

        [Fact]
        public void ConstantAlone_Throws()
        {
            const string c = "something";
#pragma warning disable CS1718 // Comparison made to same variable
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => c == c;
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.Throws<NotImplementedException>(() => FilterCompiler.CompileFromLinq(expr));
        }


        [Fact]
        public void InlineConstant_CompilesToValue()
        {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => u.SamAccountName == "something";
            var result = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal("(samaccountname=something)", result);
        }


        [Fact]
        public void Constant_CompilesToValue()
        {
            const string c = "something";
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => u.SamAccountName == c;
            var result = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal("(samaccountname=something)", result);
        }
    }
}
