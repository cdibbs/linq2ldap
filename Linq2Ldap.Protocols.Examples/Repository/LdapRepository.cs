using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using Linq2Ldap.Core.Models;
using Linq2Ldap.ExampleCommon.Specifications;

namespace Linq2Ldap.Protocols.Examples.Repository
{
    public class LdapRepository : ILdapRepository
    {
        protected ILinq2LdapConnection Connection { get; set; }
        protected string DistinguishedName { get; set; }
        protected SearchScope Scope { get; set; }

        public LdapRepository(ILinq2LdapConnection conn, string dn, SearchScope scope)
        {
            Connection = conn;
            DistinguishedName = dn;
            Scope = scope;
        }

        public T FindOne<T>(Specification<T> spec)
            where T: IEntry, new()
        {
            return FindAll(spec, 1).FirstOrDefault();
        }

        public T[] FindAll<T>(Specification<T> spec, int? limit = null)
            where T: IEntry, new()
        {
            var search = new LinqSearchRequest<T>(DistinguishedName, spec.AsExpression(), Scope);
            if (limit != null) {
                search.SizeLimit = limit.Value;
            }
            var result = Connection.SendRequest(search);
            return result.Entries.ToArray();
        }

        /// <summary>
        /// Pages through LDAP results filtered by Linq spec.
        /// Caveats:
        /// (1) paging iterates through all prior pages - a limitation of LDAP,
        /// (2) sorting can only take one attribute, and that attribute must be
        /// indexed in LDAP, or the server-side sort will fail. 
        /// </summary>
        /// <typeparam name="T">The mapped type.</typeparam>
        /// <param name="spec">The filter specification.</param>
        /// <param name="offsetPage">How many pages into the results. 0 = first page.</param>
        /// <param name="pageSize">Size of a page. Default = 10.</param>
        /// <param name="sortKeys">Sorting options.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> PageWithPageControl<T>(
            Specification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortKey[] sortKeys = null
        )
            where T : IEntry, new()
        {
            LinqSearchResponse<T> result = null;
            int curPage = 0;
            while (curPage++ <= offsetPage)
            {
                var search = new LinqSearchRequest<T>(DistinguishedName, spec.AsExpression(), Scope);
                var pageControl = new PageResultRequestControl(pageSize);
                var soc = new SearchOptionsControl(SearchOption.DomainScope);
                search.Controls.Add(pageControl);
                search.Controls.Add(soc);
                if (sortKeys != null)
                {
                    var sortControl = new SortRequestControl(sortKeys);
                    search.Controls.Add(sortControl);
                }
                result = Connection.SendRequest(search);
            }

            return result?.Entries;
        }

        /// <summary>
        /// Pages through LDAP results filtered by Linq spec.
        /// Caveats:
        /// (1) paging iterates through all prior pages - a limitation of LDAP,
        /// (2) sorting can only take one attribute, and that attribute must be
        /// indexed in LDAP, or the server-side sort will fail. 
        /// </summary>
        /// <typeparam name="T">The mapped type.</typeparam>
        /// <param name="spec">The filter specification.</param>
        /// <param name="offsetPage">How many pages into the results. 0 = first page.</param>
        /// <param name="pageSize">Size of a page. Default = 10.</param>
        /// <param name="sortKeys">Sorting options.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> PageWithVLV<T>(
            Specification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortKey[] sortKeys = null
        )
            where T : IEntry, new()
        {
            var search = new LinqSearchRequest<T>(DistinguishedName, spec.AsExpression(), Scope);
            var pageControl = new VlvRequestControl(0, pageSize - 1, pageSize * offsetPage + 1);
            var soc = new SearchOptionsControl(SearchOption.DomainScope);
            search.Controls.Add(pageControl);
            search.Controls.Add(soc);
            if (sortKeys != null)
            {
                var sortControl = new SortRequestControl(sortKeys);
                search.Controls.Add(sortControl);
            }
            return Connection.SendRequest(search).Entries;
        }

        /// <summary>
        /// Pages through LDAP results filtered by Linq spec.
        /// Caveats:
        /// (1) paging iterates through all prior pages - a limitation of LDAP,
        /// (2) sorting can only take one attribute, and that attribute must be
        /// indexed in LDAP, or the server-side sort will fail. 
        /// </summary>
        /// <typeparam name="T">The mapped type.</typeparam>
        /// <param name="spec">The filter specification.</param>
        /// <param name="offsetPage">How many pages into the results. 0 = first page.</param>
        /// <param name="pageSize">Size of a page. Default = 10.</param>
        /// <param name="sortKeys">Sorting options.</param>
        /// <param name="withVlv">Whether to use VLV paging, or the slower PageControl paging.</param>
        /// <returns>The page of results matching the spec.</returns>
        public IEnumerable<T> Page<T>(
            Specification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortKey[] sortKeys = null,
            bool withVlv = true
        )
            where T : IEntry, new()
        {
            if (withVlv)
            {
                return PageWithVLV(spec, offsetPage, pageSize, sortKeys);
            }

            return PageWithPageControl(spec, offsetPage, pageSize, sortKeys);
        }

        public void Add<T>(T entity)
            where T: IEntry, new()
        {
            var schemaClassName = "top";
            var add = new AddRequest(entity.DistinguishedName, schemaClassName);
            foreach (var k in entity.Attributes.Keys) {
                add.Attributes.Add(new DirectoryAttribute(k, entity.Attributes[k].ToArray()));
            }
            var rawConn = Connection as LdapConnection;
            var resp = rawConn.SendRequest(add) as AddResponse;
        }

        public void Update<T>(T entity)
            where T: IEntry, new()
        {
            /*var result = Searcher.FindOne();
            result.GetDirectoryEntry().Properties["something"].Value = "something";*/
            throw new NotImplementedException();
        }
    }
}
