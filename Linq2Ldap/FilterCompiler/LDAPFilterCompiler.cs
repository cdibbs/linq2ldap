using System;
using System.Linq.Expressions;
using Linq2Ldap.FilterCompiler;

namespace Linq2Ldap
{
    public class LDAPFilterCompiler : ILDAPFilterCompiler
    {
        protected CompilerCore Core { get; }

        public LDAPFilterCompiler(CompilerCore core = null)
        {
            Core = core ?? new CompilerCore();
        }

        /// <summary>
        /// Recursively compiles an Expression into an LDAP filter string (RFC 1960).
        /// See formal definition of RFC 1960 at the bottom of this Microsoft doc page.
        /// https://docs.microsoft.com/en-us/windows/desktop/adsi/search-filter-syntax
        /// </summary>
        /// <param name="expr">A filter expression.</param>
        /// <returns>The RFC 1960 filter string.</returns>
        public string CompileFromLinq<T>(Expression<Func<T, bool>> expr)
        {
            return Core.ExpressionToString(expr.Body, expr.Parameters);
        }
    }
}
