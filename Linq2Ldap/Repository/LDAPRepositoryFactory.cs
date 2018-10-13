using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;

[assembly: InternalsVisibleTo("Linq2Ldap.Tests")]
namespace Linq2Ldap.Repository
{
    public class LDAPRepositoryFactory
    {
        public IMapper Mapper { get; set; }
        public LDAPRepositoryFactory()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            Mapper = mapperConfig.CreateMapper();
        }

        public LDAPRepository Build()
        {
            return new LDAPRepository(Mapper, new LDAPFilterCompiler());
        }
    }
}
