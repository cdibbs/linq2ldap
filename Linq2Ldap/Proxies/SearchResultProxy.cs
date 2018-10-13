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

        public SearchResultProxy(SearchResult result)
        {
            Result = result;
            Properties = new ResultPropertyCollectionProxy(Result.Properties);
            Path = Result.Path;
        }

        public string Path { get; set; }

        public ResultPropertyCollectionProxy Properties { get; set; }
    }
}
