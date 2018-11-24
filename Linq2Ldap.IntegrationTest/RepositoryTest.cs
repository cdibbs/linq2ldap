using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using Linq2Ldap.Examples.Repository;
using Xunit;
using Linq2Ldap.Core.FilterCompiler;

namespace Linq2Ldap.IntegrationTest
{
    public class RepositoryTest
    {
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