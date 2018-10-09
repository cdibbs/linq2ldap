using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class BooleanAlgebraTests
    {
        private LDAPFilterCompiler FilterCompiler;
        public BooleanAlgebraTests()
        {
            FilterCompiler = new LDAPFilterCompiler();
        }

        [Fact]
        public void And_GeneratesValidFilterString()
        {
            Expression<Func<User, bool>> expr = ((User u)
                => u.CommonName.StartsWith("one") && u.CommonName.EndsWith("two"));
            var actual = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal("(&(cn=one*)(cn=*two))", actual);
        }

        [Fact]
        public void NotAnd_GeneratesValidFilterString()
        {
            Expression<Func<User, bool>> expr = ((User u)
                => !(u.CommonName.StartsWith("one") && u.CommonName.EndsWith("two")));
            var actual = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal("(!(&(cn=one*)(cn=*two)))", actual);
        }

        [Fact]
        public void Or_GeneratesValidFilterString()
        {
            Expression<Func<User, bool>> expr = ((User u)
                => u.CommonName.StartsWith("one") || u.CommonName.EndsWith("two"));
            var actual = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal("(|(cn=one*)(cn=*two))", actual);
        }
    }
}
