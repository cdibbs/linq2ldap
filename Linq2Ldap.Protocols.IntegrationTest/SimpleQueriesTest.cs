using Linq2Ldap.Core.FilterCompiler;
using Linq2Ldap.Core.ExtensionMethods;
using Linq2Ldap.TestCommon.Models;
using System.DirectoryServices.Protocols;
using System.Net;
using Xunit;

namespace Linq2Ldap.Protocols.IntegrationTest
{
    public class SimpleQueriesTest
    {
        [Fact]
        public void Protocols_BasicSearch()
        {
            var cred = new NetworkCredential()
            {
                UserName = "cn=neoman,dc=example,dc=com",
                Password = "testtest"
            };
            var ldapId = new LdapDirectoryIdentifier("127.0.0.1:1389");
            var conn = new Linq2LdapConnection(ldapId, cred, AuthType.Basic);
            var search = new LinqSearchRequest<MyModel>(
                "dc=example, dc=com",
                m => m.Mail.StartsWith("user3"),
                SearchScope.Subtree);

            // Use a more bare-bones entry type from Core unless they want to derive from the protocols-specific base type.
            // Docu.
            var response = conn.SendRequest(search);
            var user3 = response.Entries[0];
            Assert.NotNull(user3);
            Assert.Equal("mail=user3@example.com,dc=example,dc=com", user3.DistinguishedName);
        }

        [Fact]
        public void Protocols_BeginRequestEndRequest()
        {

        }

        [Fact]
        public void Protocols_ExtendedModels_SetExtendedProperties()
        {

        }
    }
}
