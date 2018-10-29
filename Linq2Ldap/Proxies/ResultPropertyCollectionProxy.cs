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

        public virtual ICollection Keys {
            get {
                if (ProxyResults != null) {
                    return ProxyResults.PropertyNames;
                }

                return Results.Keys;
            }
        }

        public virtual ICollection Values {
            get {
                if (ProxyResults != null) {
                    return ProxyResults.Values;
                }

                return Results.Values;
            }
        }

        public ResultPropertyValueCollectionProxy this[string name]
        {
            get
            {
                if (ProxyResults != null && ProxyResults.Contains(name)) {
                    return new ResultPropertyValueCollectionProxy(ProxyResults[name]);
                } else if (Results.ContainsKey(name)) {
                    return this.Results[name];
                }

                return null;
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
