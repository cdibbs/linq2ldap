using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Linq2Ldap.Models;
using Linq2Ldap.Examples.Repository;
using Moq;
using Specifications;
using Xunit;

namespace Linq2Ldap.IntegrationTest
{
    public class RepositoryTest
    {
        private IMapper Mapper;
        private ILDAPFilterCompiler Compiler;
        private LDAPRepository Repo;
        private DirectoryEntry Entry;
        public RepositoryTest()
        {
            Compiler = new LDAPFilterCompiler();
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<MapperProfile<Entry>>();
                c.AddCollectionMappers();
            });
            Mapper = config.CreateMapper();
            Entry = new DirectoryEntry("LDAP://localhost:389/o=example", "cn=neoman,ou=users,o=example", "testtest", AuthenticationTypes.None);
            Repo = new LDAPRepository(Entry);

        }

        [Fact]
        public void Add() {
            var req = new AddRequest("mail=user13@example.com, ou=users, o=example");
            //req.
            
        }
    }
}