using System;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.CompilerServices;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Protocols.Proxies;

[assembly: InternalsVisibleTo("Linq2Ldap.Protocols.Tests")]
namespace Linq2Ldap.Protocols {
    public class Linq2LdapConnection : LdapConnectionProxy, ILinq2LdapConnection
    {
        protected internal ILdapConnectionProxy LdapConnectionProxy { get; set; }
        public Linq2LdapConnection(LdapDirectoryIdentifier identifier) : base(identifier)
            => Setup();

        public Linq2LdapConnection(string server) : base(server)
            => Setup();

        public Linq2LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential) : base(identifier, credential)
            => Setup();

        public Linq2LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType) : base(identifier, credential, authType)
            => Setup();

        protected internal void Setup()
        {
            LdapConnectionProxy = this;
        }

        /// <inheritdoc />
        public ILinqSearchResponse<T> SendRequest<T>(LinqSearchRequest<T> request)
            where T : IEntry, new()
        {
            var native = LdapConnectionProxy.SendRequest(request) as SearchResponse;
            return new LinqSearchResponse<T>(native, native?.Entries);
        }

        /// <inheritdoc />
        public ILinqSearchResponse<T> SendRequest<T>(
            LinqSearchRequest<T> request,
            TimeSpan requestTimeout
        )
            where T : IEntry, new()
        {
            var native = LdapConnectionProxy.SendRequest(request, requestTimeout) as SearchResponse;
            return new LinqSearchResponse<T>(native, native?.Entries);
        }

        /// <inheritdoc />
        public ILinqSearchResponse<T> EndSendRequest<T>(IAsyncResult asyncResult) where T : IEntry, new()
        {
            var native = LdapConnectionProxy.EndSendRequest(asyncResult) as SearchResponse;
            return new LinqSearchResponse<T>(native, native?.Entries);
        }
    }
}