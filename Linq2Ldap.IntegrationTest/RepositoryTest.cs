using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Examples.Repository;
using Moq;
using Xunit;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core;

namespace Linq2Ldap.IntegrationTest
{
    public class RepositoryTest
    {
        private IMapper Mapper;
        private ILdapFilterCompiler Compiler;
        private LdapRepository Repo;
        private DirectoryEntry Entry;
        public RepositoryTest()
        {
            Compiler = new LdapFilterCompiler();
            Entry = new DirectoryEntry("LDAP://localhost:389/o=example", "cn=neoman,ou=users,o=example", "testtest", AuthenticationTypes.None);
            Repo = new LdapRepository(Entry);
        }

        [Fact]
        public void Add() {
            var req = new AddRequest("mail=user13@example.com, ou=users, o=example");
            //req.
            
        }
    }
}