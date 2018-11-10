using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Linq2Ldap.Core;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core.FilterParser;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Proxies;

[assembly: InternalsVisibleTo("Linq2Ldap.Tests")]
namespace Linq2Ldap
{
    /// <inheritdoc />
    public class LinqDirectorySearcher<T> : DirectorySearcherProxy, ILinqDirectorySearcher<T>
        where T: IEntry, new()
    {
        // Opens base to testing.
        protected internal IDirectorySearcherProxy Base;
        protected internal ILdapFilterCompiler FilterCompiler { get; set; } = new LdapFilterCompiler();
        protected internal ILdapFilterParser FilterParser { get; set; } = new LdapFilterParser();
        protected internal IModelCreator ModelCreator { get; set; } = new ModelCreator();

        public LinqDirectorySearcher(): base() {
            Base = this;
        }

        public LinqDirectorySearcher(
            DirectoryEntry entry) : base(entry)
        {
            Base = this;
        }

        /// <summary>
        /// The raw, RFC 1960 filter string. Set by Filter.
        /// </summary>
        /// <value>An RFC 1960 filter string. Overrides earlier sets.</value>
        public string RawFilter {
            get => Base.Filter;
            set => Base.Filter = value;
        }

        /// <summary>
        /// Sets the RFC 1960 filter string from a LINQ Expression.
        /// </summary>
        /// <value>A LINQ Expression representing an LDAP filter.</value>
        public new Expression<Func<T, bool>> Filter {
            get => FilterParser.Parse<T>(Base.Filter);
            set => Base.Filter = FilterCompiler.Compile(value);
        }

        public virtual new IEnumerable<T> FindAll() {
            var results = Base.FindAll();
            foreach (var result in results) {
                yield return ModelCreator.Create<T>(result.Properties, result.Path);
            }
        }
        
        public virtual new T FindOne() {
            var result = Base.FindOne();
            return ModelCreator.Create<T>(result.Properties, result.Path);
        }
    }
}