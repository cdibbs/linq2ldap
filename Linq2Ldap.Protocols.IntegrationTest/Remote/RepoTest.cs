﻿using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.ExampleCommon.Specifications;
using Linq2Ldap.Protocols.Examples.Repository;
using Linq2Ldap.TestCommon.Models;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using Xunit;

namespace Linq2Ldap.Protocols.IntegrationTest.Remote
{
    public class RepoTest
    {
        LdapRepositoryFactory Factory;
        public RepoTest()
        {
            var ldapId = new LdapDirectoryIdentifier("127.0.0.1:1389");
            var cred = new NetworkCredential()
            {
                UserName = "cn=neoman,dc=example,dc=com",
                Password = "testtest"
            };
            var conn = new Linq2LdapConnection(ldapId, cred, AuthType.Basic);
            conn.SessionOptions.ProtocolVersion = 3;
            Factory = new LdapRepositoryFactory(conn);
        }

        [Fact]
        public void Page_RunsWithoutError()
        {
            var repo = Factory.Build("dc=example, dc=com", SearchScope.Subtree);
            var results = repo.Page(Specification<MyModel>.Start(m => m.Mail.StartsWith("user")), withVlv: false).ToList();
            Assert.Equal(10, results.Count);
        }

        [Fact(Skip = "disabled pending resolution of https://github.com/ldapjs/controls/issues/1")]
        public void PageWithVlv_RunsWithoutError()
        {
            var repo = Factory.Build("dc=example, dc=com", SearchScope.Subtree);
            var results = repo.PageWithVLV(
                Specification<MyModel>.Start(m => m.Mail.StartsWith("user")),
                offsetPage: 1,
                pageSize: 5,
                sortKeys: new[] { new SortKey("mail", "caseExactMatch", false) })
                .ToList();
            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void FindAll_RespectsSize()
        {
            var repo = Factory.Build("dc=example, dc=com", SearchScope.Subtree);
            var results = repo.FindAll(Specification<MyModel>.Start(m => m.Mail.StartsWith("user")), 5).ToList();
            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void Add_RunsWithoutError()
        {
            var repo = Factory.Build("dc=example, dc=com", SearchScope.Subtree);
            var e = new Entry() {
                DistinguishedName = "mail=user20@example.com, dc=example, dc=com",
                Attributes = new EntryAttributeDictionary()
                {
                    { "backup-mail", new AttributeValueList("someaddress@example.com") }
                }
            };
            repo.Add(e);
        }
    }
}
