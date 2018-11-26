using System;
using System.DirectoryServices.Protocols;
using System.Linq.Expressions;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core.FilterParser;
using Linq2Ldap.Core.Models;

namespace Linq2Ldap.Protocols
{
    /// <summary>
    /// Wraps a native Protocols search request to facilitate searching via Expressions.
    /// </summary>
    /// <typeparam name="T">The IEntry type to represent the native LDAP entry.</typeparam>
    public class LinqSearchRequest<T>: SearchRequest
        where T: IEntry
    {
        /// <summary>
        /// The parser used by the Filter property getter.
        /// </summary>
        protected internal static ILdapFilterParser FilterParser { get; set; } = new LdapFilterParser();

        /// <summary>
        /// The compiler used by the Filter property setter.
        /// </summary>
        protected internal static ILdapFilterCompiler FilterCompiler { get; set; } = new LdapFilterCompiler();

        public LinqSearchRequest(): base() {}

        /// <summary>
        /// Creates a new search request filtered by the Expression representation of an LDAP filter.
        /// </summary>
        /// <param name="distinguishedName">The search distinguishedName or dn.</param>
        /// <param name="ldapFilter">An Expression over an IEntry type.</param>
        /// <param name="searchScope">The search scope.</param>
        /// <param name="attributeList">The attributes to load.</param>
        public LinqSearchRequest(
            string distinguishedName,
            Expression<Func<T, bool>> ldapFilter,
            SearchScope searchScope,
            params string[] attributeList
        ): base(distinguishedName, FilterCompiler.Compile(ldapFilter), searchScope, attributeList)
        {
        }


        /// <summary>
        /// The search request filter represented as a LINQ Expression.
        /// </summary>
        public new Expression<Func<T, bool>> Filter {
            get {
                if (base.Filter is string f) {
                    return FilterParser.Parse<T>(f);
                }

                throw new InvalidCastException(
                    "Underlying filter type was not a string. Access the base filter by casting the LinqSearchRequest as a SearchRequest.");
            }

            set {
                base.Filter = FilterCompiler.Compile(value);
            }
        }
    }
}