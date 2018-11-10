using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Linq.Expressions;

namespace Linq2Ldap
{
    /// <summary>
    /// A wrapper around DirectorySearcher which can use Expressions to
    /// perform queries. 
    /// </summary>
    /// <typeparam name="T">The base IEntry model for queries.</typeparam>
    public interface ILinqDirectorySearcher<T>
    {
        /// <summary>
        /// The raw, RFC 1960 filter string. Set by Filter.
        /// </summary>
        /// <value>An RFC 1960 filter string. Overrides earlier sets.</value>
        string RawFilter { get; set; }

        /// <summary>
        /// Sets the RFC 1960 filter string from a LINQ Expression.
        /// </summary>
        /// <value>A LINQ Expression representing an LDAP filter.</value>
        Expression<Func<T, bool>> Filter { set; }

        /// <summary>
        /// Finds all LDAP entries matching a particular Filter, in accordance
        /// with current DirectorySearcher settings (SearchScope, VirtualListView,
        /// Sort, PropertiesToLoad, etc).
        /// </summary>
        /// <returns>An enumerable of LDAP Entry models of type T.</returns>
        IEnumerable<T> FindAll();

        /// <summary>
        /// Finds one LDAP entry corresponding to a particular Filter, in accordance
        /// with current DirectorySearcher settings (SearchScope, VirtualListView,
        /// Sort, PropertiesToLoad, etc).
        /// </summary>
        /// <returns>An LDAP Entry of type T.</returns>
        T FindOne();

        StringCollection PropertiesToLoad { get; }

        SortOption Sort { get; set; }

        SearchScope SearchScope { get; set; }

        DirectoryVirtualListView VirtualListView { get; set; }
    }
}