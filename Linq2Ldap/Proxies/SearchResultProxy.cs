using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Linq2Ldap.Proxies
{
    public class SearchResultProxy
    {
        protected SearchResult Result;

        public SearchResultProxy()
        {
        }

        public SearchResultProxy(SearchResult result, ResultPropertyCollectionProxy properties, string path)
        {
            Result = result;
            Properties = properties;
            Path = path;
        }

        public string Path { get; set; }

        public ResultPropertyCollectionProxy Properties { get; set; }

        public static implicit operator SearchResultProxy(SearchResult result)
            => new SearchResultProxy(result, result?.Properties, result?.Path);
    }
}
