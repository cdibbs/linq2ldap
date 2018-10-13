﻿using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using AutoMapper;
using Linq2Ldap.Models;
using Linq2Ldap.Proxies;
using Specifications;

namespace Linq2Ldap.Repository
{
    public class LDAPRepository : ILDAPRepository
    {
        protected IMapper Mapper { get; set; }
        protected ILDAPFilterCompiler FilterCompiler { get; set; }
        protected DirectoryEntry Entry { get; set; }

        public LDAPRepository(IMapper mapper, ILDAPFilterCompiler filterUtil)
            : this(mapper, filterUtil, Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain)).GetDirectoryEntry())
        {
        }

        public LDAPRepository(IMapper mapper, ILDAPFilterCompiler filterUtil, DirectoryEntry entry)
        {
            Mapper = mapper;
            FilterCompiler = filterUtil;
            Entry = entry;
        }

        public T FindOne<T>(ISpecification<T> spec)
        {
            var searcher = new DirectorySearcherProxy(Entry);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = FilterCompiler.CompileFromLinq(spec.AsExpression());
            var result = new SearchResultProxy(searcher.FindOne());
            /*var pnames = new string[results[0].Properties.PropertyNames.Count];
            results[0].Properties.PropertyNames.CopyTo(pnames, 0);
            var str = pnames.Join(",");*/
            return Mapper.Map<T>(result);
        }

        public T[] FindAll<T>(ISpecification<T> spec)
        {
            var searcher = new DirectorySearcherProxy(Entry);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = FilterCompiler.CompileFromLinq(spec.AsExpression());
            searcher.PropertiesToLoad.Clear();
            searcher.PropertiesToLoad.Add("mail");
            var results = new SearchResultCollectionProxy(searcher.FindAll());
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
        public T[] Page<T>(
            ISpecification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortOption sortOpt = null)
            where T : Entry
        {
            var searcher = new DirectorySearcherProxy(Entry);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.Filter = FilterCompiler.CompileFromLinq(spec.AsExpression());
            searcher.VirtualListView = new DirectoryVirtualListView(0, pageSize - 1, pageSize * offsetPage);
            if (sortOpt != null)
                searcher.Sort = sortOpt;

            var results = searcher.FindAll();
            return Mapper.Map<T[]>(results);
        }

        public void Add<T>(T entity)
        {
            var name = "o=example";
            var schemaClassName = "top";
            var newEntry = Entry.Children.Add(name, schemaClassName);
            newEntry.Properties["mail"].Add("something@example.com");
            newEntry.CommitChanges();            
        }

        public void Update<T>(T entity)
        {
            /*var result = Searcher.FindOne();
            result.GetDirectoryEntry().Properties["something"].Value = "something";*/

            throw new NotImplementedException();
        }
    }
}