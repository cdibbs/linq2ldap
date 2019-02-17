using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Types;
using Linq2Ldap.ExampleCommon.Specifications;
using Linq2Ldap.Protocols.Examples.Repository;
using Linq2Ldap.TestCommon.Models;
using Xunit;

namespace Linq2Ldap.Protocols.IntegrationTest.Remote
{
    public class InformativeErrorsTest
    {
        LdapRepositoryFactory _factory;
        public InformativeErrorsTest()
        {
            var ldapId = new LdapDirectoryIdentifier("127.0.0.1:1389");
            var cred = new NetworkCredential()
            {
                UserName = "cn=neoman,dc=example,dc=com",
                Password = "testtest"
            };
            var conn = new Linq2LdapConnection(ldapId, cred, AuthType.Basic);
            conn.SessionOptions.ProtocolVersion = 3;
            _factory = new LdapRepositoryFactory(conn);
        }

        [Fact]
        public void ModelInstantiationPropertyError_InformsOfProperty()
        {
            var repo = _factory.Build("dc=example, dc=com", SearchScope.Subtree);
            var ex = Assert.Throws<ArgumentException>(
                () => repo.FindOne(Specification<BadUserModel>.Start(u => u["Mail"].StartsWith("user"))));

            Assert.Contains("'bogus'", ex.Message);
            Assert.Contains("'NonExistentButNonOptional'", ex.Message);
            Assert.Contains(typeof(BadUserModel).FullName, ex.Message);
            Assert.NotNull(ex.InnerException);
        }

        public class BadUserModel: Entry
        {
            [LdapField("bogus")]
            public string NonExistentButNonOptional { get; set; }
        }
    }
}
