using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Linq2Ldap.IntegrationTest.RemoteFilterEvaluation;
using Linq2Ldap.Models;
using Moq;
using Newtonsoft.Json;
using Specifications;
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
        public void CheckKnownADMemberships()
        {
            // Setup
            //var domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain));
            var entry = new DirectoryEntry("LDAP://127.0.0.1:1389", "cn=neoman,ou=users,dc=example,dc=com", "testtest", AuthenticationTypes.None);
            /*var cred = new NetworkCredential() {
                UserName = "cn=neoman,ou=users,dc=example,dc=com",
                Password = "testtest"
            };
            var ldapId = new LdapDirectoryIdentifier("127.0.0.1:1389");
            var conn = new LdapConnection(ldapId, cred, AuthType.Basic);
            var search = new SearchRequest("ou=users, dc=example, dc=com", "(mail=*)", ProtocolsSearchScope.Subtree);
            var response = conn.SendRequest(search) as SearchResponse;
            throw new Exception($"{response.Entries.Count}" + JsonConvert.SerializeObject(response.Entries[0]));*/

            //var ctx = new DirectorySearcher(entry);
            var ctx = new LinqDirectorySearcher<MyModel>(entry);
            //ctx.Filter = "(mail=user*)";
            ctx.Filter = u => u.Properties["mail"].StartsWith("user");
            //ctx.SearchRoot = new DirectoryEntry() { Path = "ou=example" };
            ctx.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            //throw new Exception($"{ctx.SearchRoot.Path}");
            var results = ctx.FindAll();
            throw new Exception(JsonConvert.SerializeObject(results));
        }
    }
}
