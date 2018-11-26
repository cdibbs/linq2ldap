using Linq2Ldap.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

// https://stackoverflow.com/questions/15733900/dynamically-creating-a-proxy-class
// https://github.com/dotnet/corefx/blob/master/src/System.DirectoryServices.Protocols/src/System/DirectoryServices/Protocols/common/DirectoryResponse.cs
namespace Linq2Ldap.Protocols
{
    /// <summary>
    /// A wrapper around a native Protocols SearchResponse that converts the entries to
    /// objects of type T.
    /// </summary>
    /// <typeparam name="T">The IEntry type to map entries.</typeparam>
    public class LinqSearchResponse<T>
        where T: IEntry, new()
    {
        /// <summary>
        /// Creates a new LinqSearchResponse from a base response and that response's entries.
        /// </summary>
        /// <param name="response">The native search response object.</param>
        /// <param name="entries">The entries from the native search response.</param>
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

        /// <summary>
        /// The native search response object.
        /// </summary>
        public virtual SearchResponse Native { get; }

        /// <summary>
        /// The native search response entries mapped to objects of type T.
        /// </summary>
        public virtual List<T> Entries { get; }
    }
}