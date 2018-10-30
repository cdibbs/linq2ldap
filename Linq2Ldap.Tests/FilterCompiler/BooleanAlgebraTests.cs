using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.FilterCompiler;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class BooleanAlgebraTests
    {
        private LdapFilterCompiler FilterCompiler;
        public BooleanAlgebraTests()
        {
            FilterCompiler = new LdapFilterCompiler();
        }

        [Fact]
        public void And_GeneratesValidFilterString()
        {
            Expression<Func<TestLdapModel, bool>> expr = ((TestLdapModel u)
                => u.CommonName.StartsWith("one") && u.CommonName.EndsWith("two"));
            var actual = FilterCompiler.Compile(expr);
            Assert.Equal("(&(cn=one*)(cn=*two))", actual);
        }

        [Fact]
        public void NotAnd_GeneratesValidFilterString()
        {
            Expression<Func<TestLdapModel, bool>> expr = ((TestLdapModel u)
                => !(u.CommonName.StartsWith("one") && u.CommonName.EndsWith("two")));
            var actual = FilterCompiler.Compile(expr);
            Assert.Equal("(!(&(cn=one*)(cn=*two)))", actual);
        }

        [Fact]
        public void Or_GeneratesValidFilterString()
        {
            Expression<Func<TestLdapModel, bool>> expr = ((TestLdapModel u)
                => u.CommonName.StartsWith("one") || u.CommonName.EndsWith("two"));
            var actual = FilterCompiler.Compile(expr);
            Assert.Equal("(|(cn=one*)(cn=*two))", actual);
        }
    }
}
