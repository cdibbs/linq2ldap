using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Linq2Ldap.Models;
using Xunit;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.FilterCompiler;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class ComparisonTests
    {
        private LdapFilterCompiler FilterCompiler;
        public ComparisonTests()
        {
            FilterCompiler = new LdapFilterCompiler();
        }

        [Fact(Skip = "Need to implement (experimental expr evaluation to determine left v right?)")]
        public void SimpleComparisons_PutsMemberRefOnLeft() {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => "something" == u.GivenName;
            var b = FilterCompiler.Compile(expr);
            var result = FilterCompiler.Compile(expr);
            Assert.Equal("(givenname=something)", result);
        }

        [Fact]
        public void CompareTo_BothSidesReferenceDataSourceModel_Throws()
        {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => u.Email.CompareTo(u.SamAccountName) > 0;
            Assert.Throws<NotImplementedException>(() => FilterCompiler.Compile(expr));
        }

        [Fact]
        public void Compare_BothSidesReferenceDataSourceModel_Throws()
        {
            Expression<Func<TestLdapModel, bool>> expr = (TestLdapModel u) => String.Compare(u.Email, u.SamAccountName) > 0;
            Assert.Throws<NotImplementedException>(() => FilterCompiler.Compile(expr));
        }

        [MemberData(nameof(StringOpData))]
        [Theory]
        public void Comparisons_GeneratesValidLDAPFilterString(Expression<Func<TestLdapModel, bool>> expr, string expected)
        {
            var actual = FilterCompiler.Compile(expr);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CustomType_CompilesToCorrectStrings() {
            Func<Expression<Func<TestLdapModel, bool>>, string> c = FilterCompiler.Compile<TestLdapModel>;
            Assert.Equal("(id=314)", c(m => m.Id == 314));
            Assert.Equal("(id>=314)", c(m => m.Id >= 314));
            Assert.Equal("(!(id<=314))", c(m => m.Id > 314));
            Assert.Equal("(id=*31*)", c(m => m.Id.Contains("31")));
            Assert.Equal("(id=31*)", c(m => m.Id.StartsWith("31")));
            Assert.Equal("(id=*31)", c(m => m.Id.EndsWith("31")));
            Assert.Equal("(id=*)", c(m => m.Id.Matches("*")));
            Assert.Equal("(id~=*)", c(m => m.Id.Approx("*")));
        }

        [Fact]
        public void CustomManyType_CompilesToCorrectStrings() {
            Func<Expression<Func<TestLdapModel, bool>>, string> c = FilterCompiler.Compile<TestLdapModel>;
            Assert.Equal("(alt-emails=someone@example.com)", c(m => m.AltEmails == "someone@example.com"));
            Assert.Equal("(alt-emails>=someone@example.com)", c(m => m.AltEmails >= "someone@example.com"));
            Assert.Equal("(!(alt-emails<=someone@example.com))", c(m => m.AltEmails > "someone@example.com"));
            Assert.Equal("(alt-emails=*someone*)", c(m => m.AltEmails.Contains("someone")));
            Assert.Equal("(alt-emails=someone*)", c(m => m.AltEmails.StartsWith("someone")));
            Assert.Equal("(alt-emails=*someone)", c(m => m.AltEmails.EndsWith("someone")));
            Assert.Equal("(alt-emails=*)", c(m => m.AltEmails.Matches("*")));
            Assert.Equal("(alt-emails~=*)", c(m => m.AltEmails.Approx("*")));
        }

        public static IEnumerable<object[]> StringOpData()
        {
            var testv = "test123";
            var items = new List<object[]>
            {
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName == "someuser"),
                    "(cn=someuser)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName != "someuser"),
                    "(!(cn=someuser))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName == testv),
                    "(cn=test123)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName != testv),
                    "(!(cn=test123))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare(u.CommonName, "someuser") > 0),
                    "(!(cn<=someuser))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare(u.CommonName, testv) > 0),
                    "(!(cn<=test123))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare(u.CommonName, testv) < 0),
                    "(!(cn>=test123))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare(u.CommonName, "someuser") <= 0),
                    "(cn<=someuser)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare(u.CommonName, testv) >= 0),
                    "(cn>=test123)"
                },

                new object[] // Reverses param order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare("someuser", u.CommonName) >= 0),
                    "(cn<=someuser)"
                },
                new object[] // Reverses param order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => String.Compare(testv, u.CommonName) <= 0),
                    "(cn>=test123)"
                },

                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName.CompareTo("someuser") > 0),
                    "(!(cn<=someuser))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName.CompareTo(testv) > 0),
                    "(!(cn<=test123))"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName.CompareTo("someuser") <= 0),
                    "(cn<=someuser)"
                },
                new object[]
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => u.CommonName.CompareTo(testv) >= 0),
                    "(cn>=test123)"
                },

                new object[] // Reverses param order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => "someuser".CompareTo(u.CommonName) >= 0),
                    "(cn<=someuser)"
                },
                new object[] // Reverses param order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => testv.CompareTo(u.CommonName) <= 0),
                    "(cn>=test123)"
                },

                new object[] // Reverses comparison order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => 0 >= u.CommonName.CompareTo("someuser")),
                    "(cn<=someuser)"
                },
                new object[] // Reverses comparison order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => 0 <= u.CommonName.CompareTo(testv)),
                    "(cn>=test123)"
                },

                new object[] // Reverses both param and comparison order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => 0 <= "someuser".CompareTo(u.CommonName)),
                    "(cn<=someuser)"
                },
                new object[] // Reverses both param and comparison order
                {
                    (Expression<Func<TestLdapModel, bool>>) ((TestLdapModel u) => 0 >= testv.CompareTo(u.CommonName)),
                    "(cn>=test123)"
                },
            };
            return items;
        }
    }
}
