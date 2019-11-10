using System.DirectoryServices;
using System.Linq;
using Linq2Ldap.Core.ExtensionMethods;
using Linq2Ldap.TestCommon.Models;
using Xunit;

namespace Linq2Ldap.IntegrationTest.RemoteFilterEvaluation
{
    public class SimpleQueriesTest
    {
        public SimpleQueriesTest()
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
            ctx.SearchScope = SearchScope.Subtree;
            var results = ctx.FindAll();
            Assert.Equal(11, results.Count());
            Assert.Contains(results, r => r.Mail == "user6@example.com");
        }

        [Fact]
        public void DirectorySearcher_BasicSearchWithArrayAttributes() {
            var entry = new DirectoryEntry(
                "LDAP://127.0.0.1:1389", "cn=neoman,dc=example,dc=com",
                "testtest", AuthenticationTypes.None);
            var ctx = new LinqDirectorySearcher<MyModel>(entry);
            ctx.Filter = u => u.AltMails.StartsWith("user6"); // (alt-mails=user6*)
            ctx.SearchScope = SearchScope.Subtree;
            var results = ctx.FindAll();
            Assert.Single(results);
            //throw new Exception($"{string.Join(", ", results.First().AltMails)}");
            Assert.Contains(results, r => r.AltMails == "user6-backup-two@example.com");
        }

        [Fact]
        public void DirectorySearcher_UsingMatchesStar_NoThrow()
        {
            var entry = new DirectoryEntry(
                "LDAP://127.0.0.1:1389", "cn=neoman,dc=example,dc=com",
                "testtest", AuthenticationTypes.None);
            var ctx = new LinqDirectorySearcher<MyModel>(entry);
            ctx.Filter = e => e.Attributes["objectCategory"].Matches("*");
            ctx.SearchScope = SearchScope.Subtree;
            var results = ctx.FindAll();
        }

        [Fact]
        public void DirectorySearcher_BasicWithSorting()
        {

        }
    }
}
