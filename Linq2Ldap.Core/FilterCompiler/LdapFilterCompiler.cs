using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core.Models;

[assembly: InternalsVisibleTo("Linq2Ldap.Core.Tests")]
namespace Linq2Ldap.Core.FilterCompiler
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
