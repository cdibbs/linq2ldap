using Linq2Ldap.Core.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Text;
using Linq2Ldap.Protocols.Proxies;

namespace Linq2Ldap.Protocols
{
    /// <summary>
    /// A wrapper around the native Protocols connection object that facilitates sending
    /// LinqSearchRequest&lt;T&gt; requests.
    /// </summary>
    public interface ILinq2LdapConnection: ILdapConnectionProxy
    {
        /// <summary>
        /// Send a wrapped LINQ Expression-based request for a wrapped response that 
        /// converts any found entries to objects of type T.
        /// </summary>
        /// <typeparam name="T">A class implementing IEntry to represent the found entries.</typeparam>
        /// <param name="request">The LINQ search request definition.</param>
        /// <returns>A wrapped response with entries mapped to IEntry objects of type T.</returns>
        ILinqSearchResponse<T> SendRequest<T>(LinqSearchRequest<T> request)
            where T : IEntry, new();

        /// <summary>
        /// Send a wrapped LINQ Expression-based request for a wrapped response that 
        /// converts any found entries to objects of type T.
        /// </summary>
        /// <typeparam name="T">A class implementing IEntry to represent the found entries.</typeparam>
        /// <param name="request">The LINQ search request definition.</param>
        /// <param name="requestTimeout">The request timeout.</param>
        /// <returns>A wrapped response with entries mapped to IEntry objects of type T.</returns>
        ILinqSearchResponse<T> SendRequest<T>(
            LinqSearchRequest<T> request,
            TimeSpan requestTimeout)
            where T : IEntry, new();

        /// <summary>
        /// Completes an asynchronous request.
        /// To be used with BeginSearchRequest(DirectoryRequest, ...).
        /// </summary>
        /// <param name="asyncResult">Contains state data for this request.</param>
        /// <returns>A wrapped, typed search response.</returns>
        ILinqSearchResponse<T> EndSendRequest<T>(IAsyncResult asyncResult)
            where T : IEntry, new();
    }
}
