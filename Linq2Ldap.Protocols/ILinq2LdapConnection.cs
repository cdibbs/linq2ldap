using Linq2Ldap.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Protocols
{
    /// <summary>
    /// A wrapper around the native Protocols connection object that facilitates sending
    /// LinqSearchRequest&lt;T&gt; requests.
    /// </summary>
    public interface ILinq2LdapConnection
    {
        /// <summary>
        /// Send a wrapped LINQ Expression-based request for a wrapped response that 
        /// converts any found entries to objects of type T.
        /// </summary>
        /// <typeparam name="T">A class implementing IEntry to represent the found entries.</typeparam>
        /// <param name="request">The LINQ search request definition.</param>
        /// <returns>A wrapped response with entries mapped to IEntry objects of type T.</returns>
        LinqSearchResponse<T> SendRequest<T>(LinqSearchRequest<T> request)
            where T : IEntry, new();
    }
}
