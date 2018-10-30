using System;
using System.DirectoryServices.Protocols;
using System.Linq.Expressions;
using Linq2Ldap.FilterCompiler;
using Linq2Ldap.FilterParser;
using Linq2Ldap.Models;

namespace Linq2Ldap.Protocols
{
    public class LinqSearchRequest<T>: SearchRequest
        where T: IEntry
    {
        internal static ILdapFilterParser FilterParser { get; set; } = new LdapFilterParser();
        internal static ILdapFilterCompiler FilterCompiler { get; set; } = new LdapFilterCompiler();
        public LinqSearchRequest(): base() {}
        public LinqSearchRequest(
            string distinguishedName,
            Expression<Func<T, bool>> ldapFilter,
            SearchScope searchScope,
            params string[] attributeList
        ): base(distinguishedName, FilterCompiler.Compile(ldapFilter), searchScope, attributeList)
        {
        }

        public object RawFilter {
            get => base.Filter;
            set => base.Filter = value;
        }

        public new Expression<Func<T, bool>> Filter {
            get {
                if (base.Filter is string f) {
                    return FilterParser.Parse<T>(f);
                }

                throw new InvalidCastException(
                    "Underlying filter type was not a string. Access the filter via RawFilter, instead.");
            }

            set {
                RawFilter = FilterCompiler.Compile(value);
            }
        }
    }
}