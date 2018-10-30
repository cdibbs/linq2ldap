using System;
using System.Linq.Expressions;
using Linq2Ldap.FilterCompiler;
using Linq2Ldap.Models;

namespace Linq2Ldap.FilterCompiler
{
    /// <inheritdoc />
    public class LdapFilterCompiler : ILdapFilterCompiler
    {
        protected CompilerCore Core { get; }

        public LdapFilterCompiler(CompilerCore core = null)
        {
            Core = core ?? new CompilerCore();
        }

        /// <inheritdoc />
        public string Compile<T>(Expression<Func<T, bool>> expr)
            where T: IEntry
        {
            return Core.ExpressionToString(expr.Body, expr.Parameters);
        }
    }
}
