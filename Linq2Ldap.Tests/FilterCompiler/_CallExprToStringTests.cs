using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.Models;
using Xunit;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.FilterCompiler;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class _CallExprToStringTests
    {
        private LdapFilterCompiler FilterCompiler;
        public _CallExprToStringTests()
        {
            FilterCompiler = new LdapFilterCompiler();
        }

        [Fact]
        public void CallStrOp_ThrowsOnUnrecognized()
        {
            Expression<Func<TestLdapModel, bool>> expr = TestLdapModel => TestLdapModel.Email.LastIndexOf("asdf") > 1;
            Action lambda = () => FilterCompiler.CompileFromLinq(expr);
            Assert.Throws<NotImplementedException>(lambda);
        }

        [Fact]
        public void Contains_ThrowsOnExtraParams()
        {
            Expression<Func<TestLdapModel, bool>> expr = TestLdapModel => TestLdapModel.Email.Contains("asdf", StringComparison.CurrentCulture);
            Action lambda = () => FilterCompiler.CompileFromLinq(expr);
            Assert.Throws<NotImplementedException>(lambda);
        }

        [MemberData(nameof(StringOpData))]
        [Theory]
        public void CallStrOp_GeneratesValidLDAPFilterString(
            Expression<Func<TestLdapModel, bool>> expr, string expected)
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
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.Contains('c')),
                    "(samaccountname=*c*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.Contains("test")),
                    "(samaccountname=*test*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.Contains("te" + "st")),
                    "(samaccountname=*test*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.Contains(testv)),
                    $"(samaccountname=*test123*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.StartsWith("test")),
                    "(samaccountname=test*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.StartsWith("te" + "st")),
                    "(samaccountname=test*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.StartsWith(testv)),
                    $"(samaccountname=test123*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.EndsWith("test")),
                    "(samaccountname=*test)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.EndsWith("te" + "st")),
                    "(samaccountname=*test)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.EndsWith(testv)),
                    $"(samaccountname=*test123)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.Has("key")),
                    $"(key=*)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.Properties[member] == "123"),
                    $"(samaccountname=123)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u["samaccountname"] == "123"),
                    $"(samaccountname=123)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u[member] == "123"),
                    $"(samaccountname=123)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u["samaccountname"] == "123"),
                    $"(samaccountname=123)"
                },

                new object[] {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.SamAccountName.Matches("univ*of*iowa")),
                    $"(samaccountname=univ*of*iowa)"
                }
            };
            return items;
        }
    }
}
