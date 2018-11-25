using Linq2Ldap.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

// https://stackoverflow.com/questions/15733900/dynamically-creating-a-proxy-class
// https://github.com/dotnet/corefx/blob/master/src/System.DirectoryServices.Protocols/src/System/DirectoryServices/Protocols/common/DirectoryResponse.cs
namespace Linq2Ldap.Protocols
{
    public class LinqSearchResponse<T>
        where T: IEntry, new()
    {
        public LinqSearchResponse(SearchResponse response, IEnumerable entries)
        {
            Native = response;
            Entries = new List<T>();
            if (entries == null) return;
            foreach (dynamic entry in entries)
            {
                var converted = new T()
                {
                    DistinguishedName = entry.DistinguishedName,
                    Attributes = entry.Attributes
                };

                if (converted is LinqSearchResultEntry sre)
                {
                    sre.Native = entry;
                    sre.Controls = entry.Controls;
                }

                Entries.Add(converted);
            }
        }

        public virtual SearchResponse Native { get; }

        public virtual List<T> Entries { get; }
    }
}