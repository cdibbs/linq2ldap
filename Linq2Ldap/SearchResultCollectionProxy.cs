using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Linq2Ldap
{
    public class SearchResultCollectionProxy: IEnumerable<SearchResultProxy>
    {
        protected SearchResultCollection Results;
        public SearchResultCollectionProxy(SearchResultCollection results)
        {
            Results = results;
        }

        public IEnumerator<SearchResultProxy> GetEnumerator()
        {
            for (var i = 0; i < Results.Count; i++)
            {
                yield return new SearchResultProxy(Results[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
