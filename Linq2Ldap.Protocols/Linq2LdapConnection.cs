using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.CompilerServices;
using Linq2Ldap.Core.Models;

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

        public LinqSearchResponse<T> SendRequest<T>(LinqSearchRequest<T> request)
            where T : IEntry, new()
        {
            var native = LdapConnectionProxy.SendRequest(request) as SearchResponse;
            return new LinqSearchResponse<T>(native, native?.Entries);
        }
    }
}