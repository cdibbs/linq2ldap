using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.Text;
using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Proxies
{
    public class SearchResultProxy
    {
        protected SearchResult Result;

        public SearchResultProxy()
        {
        }

        public SearchResultProxy(SearchResult result, DirectoryEntryPropertyCollection properties, string path)
        {
            Result = result;
            Properties = properties;
            Path = path;
        }

        public string Path { get; set; }

        public DirectoryEntryPropertyCollection Properties { get; set; }

        [ExcludeFromCodeCoverage]
        public static implicit operator SearchResultProxy(SearchResult result)
            => result == null ? null : new SearchResultProxy(result, result.Properties, result.Path);
    }
}
