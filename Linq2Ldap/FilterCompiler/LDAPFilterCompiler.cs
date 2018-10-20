using System;
using System.Linq.Expressions;
using Linq2Ldap.FilterCompiler;

namespace Linq2Ldap.FilterCompiler
{
    /// <inheritdoc />
    public class LDAPFilterCompiler : ILDAPFilterCompiler
    {
        protected CompilerCore Core { get; }

        public LDAPFilterCompiler(CompilerCore core = null)
        {
            Core = core ?? new CompilerCore();
        }

        /// <inheritdoc />
        public string CompileFromLinq<T>(Expression<Func<T, bool>> expr)
        {
            return Core.ExpressionToString(expr.Body, expr.Parameters);
        }
    }
}
