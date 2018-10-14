using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Linq.Expressions;
using AutoMapper;
using Linq2Ldap.Models;
using Linq2Ldap.Proxies;

namespace Linq2Ldap
{
    public interface ILinqDirectorySearcher<T>
    {
        string RawFilter { get; set; }


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