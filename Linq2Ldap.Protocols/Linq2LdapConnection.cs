using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.CompilerServices;
using Linq2Ldap.Core.Models;

[assembly: InternalsVisibleTo("Linq2Ldap.Protocols.Tests")]
namespace Linq2Ldap.Protocols {
    public class Linq2LdapConnection : LdapConnection
    {
        public Linq2LdapConnection(LdapDirectoryIdentifier identifier) : base(identifier)
        {
        }

        public Linq2LdapConnection(string server) : base(server)
        {
        }

        public Linq2LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential) : base(identifier, credential)
        {
        }

        public Linq2LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType) : base(identifier, credential, authType)
        {
        }

        public LinqSearchResponse<T> SendRequest<T>(LinqSearchRequest<T> request)
            where T: LinqSearchResultEntry, new()
            => new LinqSearchResponse<T>(base.SendRequest(request) as SearchResponse);
    }
}