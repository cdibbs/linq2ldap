using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Linq2Ldap.Core.Models;

namespace Linq2Ldap.Protocols
{
    public interface ILinqSearchResponse<T> where T : IEntry, new()
    {
        /// <summary>
        /// The native search response object.
        /// </summary>
        SearchResponse Native { get; }

        /// <summary>
        /// The native search response entries mapped to objects of type T.
        /// </summary>
        List<T> Entries { get; }
    }
}