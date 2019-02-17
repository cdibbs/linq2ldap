using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Models;
using Linq2Ldap.TestCommon.Models;
using Xunit;

namespace Linq2Ldap.IntegrationTest.RemoteFilterEvaluation
{
    public class InformativeErrorTest
    {
        [Fact]
        public void ModelInstantiationPropertyError_InformsOfProperty()
        {
            var entry = new DirectoryEntry(
                "LDAP://127.0.0.1:1389", "cn=neoman,dc=example,dc=com",
                "testtest", AuthenticationTypes.None);
            var ctx = new LinqDirectorySearcher<BadUserModel>(entry);
            ctx.Filter = u => u["mail"].StartsWith("user");
            ctx.SearchScope = SearchScope.Subtree;
            var ex = Assert.Throws<ArgumentException>(() => ctx.FindAll().First());

            Assert.Contains("'bogus'", ex.Message);
            Assert.Contains("'NonExistentButNonOptional'", ex.Message);
            Assert.Contains(typeof(BadUserModel).FullName, ex.Message);
            Assert.NotNull(ex.InnerException);
        }

        public class BadUserModel : Entry
        {
            [LdapField("bogus")]
            public string NonExistentButNonOptional { get; set; }
        }
    }
}
