using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using AutoMapper;
using Linq2Ldap.Models;
using Specifications;

namespace Linq2Ldap
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
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="offsetPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortFactory"></param>
        /// <returns></returns>
        public T[] Page<T>(
            ISpecification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortOption sortOpt = null)
            where T : BaseSAMAccount
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
            //Domain.GetDirectoryEntry().;
            //result.GetDirectoryEntry().Properties["thing"].Add("something");*/
            throw new NotImplementedException();
        }

        public void Update<T>(T entity)
        {
            /*var result = Searcher.FindOne();
            result.GetDirectoryEntry().Properties["something"].Value = "something";*/
            throw new NotImplementedException();
        }
    }
}
