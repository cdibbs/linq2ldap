using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.IntegrationTest.Models;
using Xunit;
using ProtocolsSearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace Linq2Ldap.IntegrationTest.RemoteFilterEvaluation
{
    public class IntegrationTest
    {
        public IntegrationTest()
        {
        }

        /// <summary>
        /// Let's keep it simple with our integration test.
        /// If you want to add to this because a bug has been found,
        /// please cover it with a unit test, first, and an integration test
        /// only if absolutely necessary. Thx youuus! :-)
        /// </summary>
        [Fact]
        public void DirectorySearcher_BasicSearch()
        {
            var entry = new DirectoryEntry(
                "LDAP://127.0.0.1:1389", "cn=neoman,dc=example,dc=com",
                "testtest", AuthenticationTypes.None);
            var ctx = new LinqDirectorySearcher<MyModel>(entry);
            ctx.Filter = u => u.Mail.StartsWith("user");
            ctx.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            var results = ctx.FindAll();
            Assert.Equal(11, results.Count());
            Assert.Contains(results, r => r.Mail == "user6@example.com");
        }

        [Fact]
        public void Protocols_BasicSearch() {
            var cred = new NetworkCredential() {
                UserName = "cn=neoman,dc=example,dc=com",
                Password = "testtest"
            };
            var ldapId = new LdapDirectoryIdentifier("127.0.0.1:1389");
            var filter = new LdapFilterCompiler().Compile(
                (MyModel u) => u["mail"].StartsWith("user")
            );
            var conn = new LdapConnection(ldapId, cred, AuthType.Basic);
            var search = new SearchRequest("dc=example, dc=com", "(mail=user3*)", ProtocolsSearchScope.Subtree);
            var response = conn.SendRequest(search) as SearchResponse;
            var user3 = response.Entries[0];
            Assert.NotNull(user3);
            Assert.Equal("mail=user3@example.com, dc=example, dc=com", user3.DistinguishedName);
        }

        [Fact]
        public void DirectorySearcher_BasicSearchWithArrayAttributes() {
            var entry = new DirectoryEntry(
                "LDAP://127.0.0.1:1389", "cn=neoman,dc=example,dc=com",
                "testtest", AuthenticationTypes.None);
            var ctx = new LinqDirectorySearcher<MyModel>(entry);
            ctx.Filter = u => u.AltMails.StartsWith("user6"); // (alt-mails=user6*)
            ctx.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            var results = ctx.FindAll();
            Assert.Single(results);
            //throw new Exception($"{string.Join(", ", results.First().AltMails)}");
            Assert.Contains(results, r => r.AltMails == "user6-backup-two@example.com");
        }
    }
}
