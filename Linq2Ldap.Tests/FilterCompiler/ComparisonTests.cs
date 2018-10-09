using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class ComparisonTests
    {
        private LDAPFilterCompiler FilterCompiler;
        public ComparisonTests()
        {
            FilterCompiler = new LDAPFilterCompiler();
        }

        [Fact]
        public void CompareTo_BothSidesReferenceDataSourceModel_Throws()
        {
            Expression<Func<User, bool>> expr = (User u) => u.Email.CompareTo(u.SamAccountName) > 0;
            Assert.Throws<NotImplementedException>(() => FilterCompiler.CompileFromLinq(expr));
        }

        [Fact]
        public void Compare_BothSidesReferenceDataSourceModel_Throws()
        {
            Expression<Func<User, bool>> expr = (User u) => String.Compare(u.Email, u.SamAccountName) > 0;
            Assert.Throws<NotImplementedException>(() => FilterCompiler.CompileFromLinq(expr));
        }

        [MemberData(nameof(StringOpData))]
        [Theory]
        public void Comparisons_GeneratesValidLDAPFilterString(Expression<Func<User, bool>> expr, string expected)
        {
            var actual = FilterCompiler.CompileFromLinq(expr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> StringOpData()
        {
            var testv = "test123";
            var items = new List<object[]>
            {
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName == "someuser"),
                    "(cn=someuser)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName != "someuser"),
                    "(!(cn=someuser))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName == testv),
                    "(cn=test123)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName != testv),
                    "(!(cn=test123))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare(u.CommonName, "someuser") > 0),
                    "(!(cn<=someuser))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare(u.CommonName, testv) > 0),
                    "(!(cn<=test123))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare(u.CommonName, testv) < 0),
                    "(!(cn>=test123))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare(u.CommonName, "someuser") <= 0),
                    "(cn<=someuser)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare(u.CommonName, testv) >= 0),
                    "(cn>=test123)"
                },

                new object[] // Reverses param order
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare("someuser", u.CommonName) >= 0),
                    "(cn<=someuser)"
                },
                new object[] // Reverses param order
                {
                    (Expression<Func<User, bool>>) ((User u) => String.Compare(testv, u.CommonName) <= 0),
                    "(cn>=test123)"
                },

                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName.CompareTo("someuser") > 0),
                    "(!(cn<=someuser))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName.CompareTo(testv) > 0),
                    "(!(cn<=test123))"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName.CompareTo("someuser") <= 0),
                    "(cn<=someuser)"
                },
                new object[]
                {
                    (Expression<Func<User, bool>>) ((User u) => u.CommonName.CompareTo(testv) >= 0),
                    "(cn>=test123)"
                },

                new object[] // Reverses param order
                {
                    (Expression<Func<User, bool>>) ((User u) => "someuser".CompareTo(u.CommonName) >= 0),
                    "(cn<=someuser)"
                },
                new object[] // Reverses param order
                {
                    (Expression<Func<User, bool>>) ((User u) => testv.CompareTo(u.CommonName) <= 0),
                    "(cn>=test123)"
                },

                new object[] // Reverses comparison order
                {
                    (Expression<Func<User, bool>>) ((User u) => 0 >= u.CommonName.CompareTo("someuser")),
                    "(cn<=someuser)"
                },
                new object[] // Reverses comparison order
                {
                    (Expression<Func<User, bool>>) ((User u) => 0 <= u.CommonName.CompareTo(testv)),
                    "(cn>=test123)"
                },

                new object[] // Reverses both param and comparison order
                {
                    (Expression<Func<User, bool>>) ((User u) => 0 <= "someuser".CompareTo(u.CommonName)),
                    "(cn<=someuser)"
                },
                new object[] // Reverses both param and comparison order
                {
                    (Expression<Func<User, bool>>) ((User u) => 0 >= testv.CompareTo(u.CommonName)),
                    "(cn>=test123)"
                },
            };
            return items;
        }
    }
}
