using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.Models;
using Linq2Ldap.Types;
using Xunit;
using Linq2Ldap.ExtensionMethods;

namespace Linq2Ldap.Tests.Types {
    public class LdapStringListIntegrationTest {

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
        public void LessThan_AnyLT_True() {
            var users = new List<MyUser>() {
                new MyUser() { Attr = new [] { "123", "456" } },
                new MyUser() { Attr = new [] { "abc", "cde", "def" } },
                new MyUser() { Attr = new [] { "abc", "def" } }
            }.AsQueryable();

            var filtered = users.Where(u => u.Attr < "456");
            Assert.Single(filtered);
            Assert.Contains(filtered.First().Attr, a => a == "123");
        }

        [Fact]
        public void Matches_AnyMatches_True() {
            var users = new List<MyUser>() {
                new MyUser() { Attr = new [] { "123", "456" } },
                new MyUser() { Attr = new [] { "abc", "cde", "def" } },
                new MyUser() { Attr = new [] { "abc", "def" } }
            }.AsQueryable();

            var filtered = users.Where(u => u.Attr.Matches("4*"));
            Assert.Single(filtered);
            Assert.Contains(filtered.First().Attr, a => a == "123");
        }
    }

    public class MyUser: Entry {
        public LdapStringList Attr { get; set; }
    }
}