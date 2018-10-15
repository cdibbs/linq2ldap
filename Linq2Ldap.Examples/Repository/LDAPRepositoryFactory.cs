using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;

namespace Linq2Ldap.Examples.Repository
{
    public class LDAPRepositoryFactory
    {
        public LDAPRepositoryFactory() {}

        public LDAPRepository Build()
        {
            return new LDAPRepository();
        }

        public LDAPRepository Build(DirectoryEntry entry)
        {
            return new LDAPRepository(entry);
        }
    }
}
