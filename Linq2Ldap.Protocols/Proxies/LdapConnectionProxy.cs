using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Linq2Ldap.Core.Models;

namespace Linq2Ldap.Protocols.Proxies
{
    public class LdapConnectionProxy : LdapConnection, ILdapConnectionProxy
    {
        public LdapConnectionProxy(LdapDirectoryIdentifier identifier) : base(identifier)
        {
        }

        public LdapConnectionProxy(string server) : base(server)
        {
        }

        public LdapConnectionProxy(LdapDirectoryIdentifier identifier, NetworkCredential credential) : base(identifier, credential)
        {
        }

        public LdapConnectionProxy(LdapDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType) : base(identifier, credential, authType)
        {
        }

        public new void Dispose(bool disposing) => base.Dispose();
    }
}
