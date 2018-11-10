using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using AutoMapper;
using Linq2Ldap.Core.Models;

// https://stackoverflow.com/questions/15733900/dynamically-creating-a-proxy-class
// https://github.com/dotnet/corefx/blob/master/src/System.DirectoryServices.Protocols/src/System/DirectoryServices/Protocols/common/DirectoryResponse.cs
namespace Linq2Ldap.Protocols {
    public class LinqSearchResponse<T>
        where T: LinqSearchResultEntry, new()
    {
        public LinqSearchResponse(SearchResponse response)
        {
            Response = response;
        }

        public virtual SearchResponse Response { get; set; }

        public virtual IEnumerable<T> Entries {
            get {
                foreach (SearchResultEntry entry in Response.Entries) {
                    yield return new T() {
                        DistinguishedName = entry.DistinguishedName,
                        Attributes = entry.Attributes,
                        Native = entry,
                        Controls = entry.Controls
                    };
                }
            }
        }

    }
}