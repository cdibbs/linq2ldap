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
        public IMapper Mapper { get; set; }
        public LDAPRepositoryFactory(MapperConfiguration mapperConfig = null)
        {
            mapperConfig = mapperConfig ?? new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            Mapper = mapperConfig.CreateMapper();
        }

        public LDAPRepository Build()
        {
            return new LDAPRepository(Mapper, new LDAPFilterCompiler());
        }

        public LDAPRepository Build(DirectoryEntry entry)
        {
            return new LDAPRepository(Mapper, new LDAPFilterCompiler(), entry);
        }
    }
}
