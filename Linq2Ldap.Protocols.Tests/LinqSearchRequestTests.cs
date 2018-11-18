using System;
using System.DirectoryServices.Protocols;
using System.Linq.Expressions;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core.Models;
using Xunit;

namespace Linq2Ldap.Protocols.Tests
{
    public class LinqSearchRequestTests
    {
        [Fact]
        public void CtorLinq_SetsBaseFilterStringFromExpr()
        {
            Expression<Func<Entry, bool>> expr = m => m["123"] == "456";
            var expectedStr = new LdapFilterCompiler().Compile(expr);
            var inst = new LinqSearchRequest<Entry>("bogusdn", expr, SearchScope.Base);
            
            Assert.Equal(expectedStr, (inst as SearchRequest).Filter);
        }

        [Fact]
        public void Filter_Get_ReturnsParsedBaseFilterWhenString()
        {
            var inst = new LinqSearchRequest<Entry>();
            (inst as SearchRequest).Filter = "(asdf=jkl)";
            Expression<Func<Entry, bool>> expectedExpr = m => m["asdf"] == "jkl";
            Assert.Equal(expectedExpr.ToString(), inst.Filter.ToString());
        }

        [Fact]
        public void Filter_Get_ThrowsWhenBaseNotString()
        {
            var inst = new LinqSearchRequest<Entry>();
            (inst as SearchRequest).Filter = null;
            Assert.Throws<InvalidCastException>(() => inst.Filter);
        }

        [Fact]
        public void Filter_Set_SetsBaseFilterStringFromExpression()
        {
            Expression<Func<Entry, bool>> expr = m => m["123"] == "456";
            var expectedStr = new LdapFilterCompiler().Compile(expr);
            var inst = new LinqSearchRequest<Entry>();
            inst.Filter = expr;
            Assert.Equal(expectedStr, (inst as SearchRequest).Filter);
        }
    }
}
