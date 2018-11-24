using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Linq2Ldap.Protocols;
using Linq2Ldap.Protocols.Examples.Repository;

namespace Linq2Ldap.Protocols.Examples.Repository
{
    public class LdapRepositoryFactory
    {
        ILinq2LdapConnection Connection;
        public LdapRepositoryFactory(ILinq2LdapConnection conn)
        {
            Connection = conn;
        }

        public LdapRepository Build(string dn, SearchScope scope)
        {
            return new LdapRepository(Connection, dn, scope);
        }
    }
}
