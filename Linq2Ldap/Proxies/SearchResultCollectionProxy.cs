using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Linq2Ldap.Proxies
{
    public class SearchResultCollectionProxy: IEnumerable<SearchResultProxy>
    {
        protected IEnumerable Results;
        public SearchResultCollectionProxy(List<SearchResultProxy> mockResults) {
            Results = mockResults;
        }
        public SearchResultCollectionProxy(SearchResultCollection results)
        {
            Results = results;
        }

        public IEnumerator<SearchResultProxy> GetEnumerator()
        {
            foreach (var e in Results) {
                if (e is SearchResult sr)
                    yield return sr;
                else if (e is SearchResultProxy p)
                    yield return p;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator SearchResultCollectionProxy(SearchResultCollection col)
            => new SearchResultCollectionProxy(col);
    }
}
