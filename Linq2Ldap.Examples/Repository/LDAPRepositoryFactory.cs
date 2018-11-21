using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;

namespace Linq2Ldap.Examples.Repository
{
    public class LdapRepositoryFactory
    {
        public LdapRepositoryFactory() {}

        public LdapRepository Build()
        {
            return new LdapRepository();
        }

        public LdapRepository Build(DirectoryEntry entry)
        {
            return new LdapRepository(entry);
        }
    }
}
