using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.Models;
using Linq2Ldap.Types;
using Xunit;

namespace Linq2Ldap.Tests.Types {
    public class LDAPStringListIntegrationTest {

        [Fact]
        public void Equals_AnyEquals_True() {
            var users = new List<MyUser>() {
                new MyUser() { Attr = new [] { "123", "456" } },
                new MyUser() { Attr = new [] { "abc", "cde", "def" } },
                new MyUser() { Attr = new [] { "abc", "def" } }
            }.AsQueryable();

            var filtered = users.Where(u => u.Attr == "cde");
            Assert.Single(filtered);
            Assert.Contains(filtered.First().Attr, a => a == "cde");
        }

        [Fact]
        public void BuiltExpression_Filters() {
            var users = new List<MyUser>() {
                new MyUser() { Attr = new [] { "123", "456" } },
                new MyUser() { Attr = new [] { "abc", "cde", "def" } },
                new MyUser() { Attr = new [] { "abc", "def" } }
            }.AsQueryable();

            var filtered = users.Where(u => u.Attr < "456");
            Assert.Single(filtered);
            Assert.Contains(filtered.First().Attr, a => a == "123");
        }
    }

    public class MyUser: Entry {
        public LDAPStringList Attr { get; set; }
    }
}