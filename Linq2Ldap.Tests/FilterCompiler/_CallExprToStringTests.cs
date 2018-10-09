using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class _CallExprToStringTests
    {
        private LDAPFilterCompiler FilterCompiler;
        public _CallExprToStringTests()
        {
            FilterCompiler = new LDAPFilterCompiler();
        }

        [Fact]
        public void CallStrOp_ThrowsOnUnrecognized()
        {
            Expression<Func<User, bool>> expr = user => user.Email.LastIndexOf("asdf") > 1;
            Action lambda = () => FilterCompiler.CompileFromLinq(expr);
            Assert.Throws<NotImplementedException>(lambda);
        }

        [Fact]
        public void Contains_ThrowsOnExtraParams()
        {
            Expression<Func<User, bool>> expr = user => user.Email.Contains("asdf", StringComparison.CurrentCulture);
            Action lambda = () => FilterCompiler.CompileFromLinq(expr);
            Assert.Throws<NotImplementedException>(lambda);
        }

        [MemberData(nameof(StringOpData))]
        [Theory]
        public void CallStrOp_GeneratesValidLDAPFilterString(
            Expression<Func<User, bool>> expr, string expected)
        {
            var actual = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> StringOpData()
        {
            var testv = "test123";
            var member = "samaccountname";
            var items = new List<object[]>
            {
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.Contains('c')),
                    "(samaccountname=*c*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.Contains("test")),
                    "(samaccountname=*test*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.Contains("te" + "st")),
                    "(samaccountname=*test*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.Contains(testv)),
                    $"(samaccountname=*test123*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.StartsWith("test")),
                    "(samaccountname=test*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.StartsWith("te" + "st")),
                    "(samaccountname=test*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.StartsWith(testv)),
                    $"(samaccountname=test123*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.EndsWith("test")),
                    "(samaccountname=*test)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.EndsWith("te" + "st")),
                    "(samaccountname=*test)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.SamAccountName.EndsWith(testv)),
                    $"(samaccountname=*test123)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => !String.IsNullOrEmpty(u.Email)),
                    $"(mail=*)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.Properties[member] == "123"),
                    $"(samaccountname=123)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.Properties["samaccountname"] == "123"),
                    $"(samaccountname=123)"
                }
            };
            return items;
        }
    }
}
