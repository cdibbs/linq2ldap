using System;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Linq2Ldap.Protocols.Proxies
{
    public interface ILdapConnectionProxy
    {
        AuthType AuthType { get; set; }

        /// <summary>
        /// Will bind on first search.
        /// </summary>
        bool AutoBind { get; set; }
        NetworkCredential Credential { set; }
        LdapSessionOptions SessionOptions { get; }
        TimeSpan Timeout { get; set; }
        X509CertificateCollection ClientCertificates { get; }
        DirectoryIdentifier Directory { get; }

        void Abort(IAsyncResult asyncResult);
        IAsyncResult BeginSendRequest(DirectoryRequest request, PartialResultProcessing partialMode, AsyncCallback callback, object state);
        IAsyncResult BeginSendRequest(DirectoryRequest request, TimeSpan requestTimeout, PartialResultProcessing partialMode, AsyncCallback callback, object state);
        void Bind();
        void Bind(NetworkCredential newCredential);
        void Dispose();
        void Dispose(bool disposing);
        DirectoryResponse EndSendRequest(IAsyncResult asyncResult);
        PartialResultsCollection GetPartialResults(IAsyncResult asyncResult);
        DirectoryResponse SendRequest(DirectoryRequest request);
        DirectoryResponse SendRequest(DirectoryRequest request, TimeSpan requestTimeout);
    }
}