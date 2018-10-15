using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Linq2Ldap.Proxies
{
    public class ResultPropertyCollectionProxy: DictionaryBase
    {
        protected ResultPropertyCollection ProxyResults;
        protected Dictionary<string, ResultPropertyValueCollectionProxy> Results;

        public ResultPropertyCollectionProxy(Dictionary<string, ResultPropertyValueCollectionProxy> results)
        {
            Results = results;
        }

        public ResultPropertyCollectionProxy(ResultPropertyCollection proxyResults)
        {
            ProxyResults = proxyResults;
        }

        public virtual bool Contains(string key) {
            if (ProxyResults != null) {
                return ProxyResults.Contains(key);
            }

            return this.Results.ContainsKey(key);
        }

        public ResultPropertyValueCollectionProxy this[string name]
        {
            get
            {
                return ProxyResults != null
                    ? new ResultPropertyValueCollectionProxy(ProxyResults[name])
                    : this.Results[name];
            }
        }

        /// <summary>Gets the number of <see cref="T:System.DirectoryServices.SearchResult" /> objects in this collection.</summary>
        /// <returns>The number of <see cref="T:System.DirectoryServices.SearchResult" /> objects in this collection.</returns>
        public new int Count
        {
            get
            {
                return ProxyResults?.Count ?? this.Results.Count;
            }
        }

        public static implicit operator ResultPropertyCollectionProxy(ResultPropertyCollection col)
            => new ResultPropertyCollectionProxy(col);
    }
}
