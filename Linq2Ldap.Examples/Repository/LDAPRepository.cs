using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq.Expressions;
using AutoMapper;
using Linq2Ldap.Models;
using Linq2Ldap.Proxies;
using Linq2Ldap.Specifications;

namespace Linq2Ldap.Examples.Repository
{
    public class LDAPRepository : ILDAPRepository
    {
        protected DirectoryEntry Entry { get; set; }

        public LDAPRepository()
            : this(Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain)).GetDirectoryEntry())
        {
        }

        public LDAPRepository(DirectoryEntry entry)
        {
            Entry = entry;
        }

        public T FindOne<T>(Specification<T> spec)
            where T: class, IEntry
        {
            var searcher = new LinqDirectorySearcher<T>(Entry);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = spec;
            var result = searcher.FindOne();
            /*var pnames = new string[results[0].Properties.PropertyNames.Count];
            results[0].Properties.PropertyNames.CopyTo(pnames, 0);
            var str = pnames.Join(",");*/
            return Mapper.Map<T>(result);
        }

        public T[] FindAll<T>(Specification<T> spec)
            where T: class, IEntry
        {
            var searcher = new LinqDirectorySearcher<T>(Entry);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = spec;
            searcher.PropertiesToLoad.Clear();
            searcher.PropertiesToLoad.Add("mail");
            var results = searcher.FindAll();
            /*var pnames = new string[results[0].Properties.PropertyNames.Count];
            results[0].Properties.PropertyNames.CopyTo(pnames, 0);
            var str = pnames.Join(",");*/
            return Mapper.Map<T[]>(results);
        }

        /// <summary>
        /// Pages through LDAP results filtered by Linq spec.
        /// Caveat: sorting can only take one attribute, and that attribute
        /// must be indexed in LDAP, or the server-side sort will fail. 
        /// </summary>
        /// <typeparam name="T">The mapped type.</typeparam>
        /// <param name="spec">The filter specification.</param>
        /// <param name="offsetPage">How many pages into the results. 0 = first page.</param>
        /// <param name="pageSize">Size of a page. Default = 10.</param>
        /// <param name="sortOpt">Sorting options.</param>
        /// <returns></returns>
        public IEnumerable<T> Page<T>(
            Specification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortOption sortOpt = null)
            where T : class, IEntry
        {
            var searcher = new LinqDirectorySearcher<T>(Entry);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = spec;
            searcher.VirtualListView = new DirectoryVirtualListView(
                0, pageSize - 1, pageSize * offsetPage + 1);

            // Not obvious, but VLV must have a sort option.
            searcher.Sort = sortOpt ?? new SortOption("cn", SortDirection.Ascending);
            return searcher.FindAll();
        }

        public void Add<T>(T entity)
            where T: class, IEntry
        {
            var name = "o=example";
            var schemaClassName = "top";
            var newEntry = Entry.Children.Add(name, schemaClassName);
            newEntry.Properties["mail"].Add("something@example.com");
            newEntry.CommitChanges();            
        }

        public void Update<T>(T entity)
            where T: class, IEntry
        {
            /*var result = Searcher.FindOne();
            result.GetDirectoryEntry().Properties["something"].Value = "something";*/
            throw new NotImplementedException();
        }
    }
}
