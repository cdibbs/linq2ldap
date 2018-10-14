using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Linq2Ldap.Models;
using Moq;
using Specifications;
using Xunit;

namespace Linq2Ldap.IntegrationTest
{
    public class IntegrationTest
    {
        private IMapper Mapper;
        private ILDAPFilterCompiler FilterUtil;
        public IntegrationTest()
        {
            FilterUtil = new LDAPFilterCompiler();
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<MapperProfile>();
                c.AddCollectionMappers();
            });
            Mapper = config.CreateMapper();
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
            var entry = new DirectoryEntry("LDAP://localhost:389/o=example", "cn=neoman,ou=users,o=example", "testtest", AuthenticationTypes.None);
            var ctx = new LinqDirectorySearcher<Entry>(entry);
            ctx.Filter = u => u.Properties["mail"].StartsWith("user");
            //Assert.Null(new LDAPFilterCompiler().CompileFromLinq(Specification<BaseSAMAccount>.Start(u => u.Properties["mail"] == "*").AsExpression()));
            var results = ctx.FindAll();
            //Assert.Contains(results, u => u.SamAccountName == "estestseven");
            //Assert.Contains(results, u => u.SamAccountName == "estestfive");
            //Assert.Contains(results, u => u.SamAccountName == "estestone");

            var users1 = new[] { "estestseven", "estestfive" }; /* test users setup in AD */
            var users2 = new[] { "estestone" };
            var groups1 = new[] { "ITS-AppDev-IntegTest1", "ITS-AppDev-IntegTest2" };
            var groups2 = new[] { "ITS-AppDev-IntegTest2" };

            // Test
            /*var membersBoth = groupMgr.FilterUsersByMembership(users1, groups1);
            var membersOne = groupMgr.FilterUsersByMembership(users1, groups2);
            var membersNone = groupMgr.FilterUsersByMembership(users2, groups1);
            var resultYes = groupMgr.IsMemberOfAny("estestfive", groups1);
            var resultNo = groupMgr.IsMemberOfAny("estestone", groups2);

            // Assert
            Assert.Equal(2, membersBoth.Length); // can be all
            Assert.Single(membersOne); // can be some
            Assert.False(membersNone.Any()); // can be empty

            Assert.True(resultYes); // can be true
            Assert.False(resultNo); // can be false*/
        }
    }
}
