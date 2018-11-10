using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Linq2Ldap.Core;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.Proxies;
using Linq2Ldap.TestUtil;
using Moq;
using Xunit;

namespace Linq2Ldap.Tests
{
    public class LinqDirectorySearcherTests
    {
        IModelCreator Creator { get; set; }
        LinqDirectorySearcher<TestLdapModel> Searcher;
        public LinqDirectorySearcherTests() {
            Creator = Mock.Of<IModelCreator>();
            Searcher = new LinqDirectorySearcher<TestLdapModel>();
            Searcher.Base = Mock.Of<IDirectorySearcherProxy>();
            Searcher.ModelCreator = Creator;
        }

        [WindowsOnlyFact]
        public void FindAll_ReturnsMappedFromBase() {
            var first = new SearchResultProxy()
            {
                Path = "ou=some,com=path",
                Properties = new DirectoryEntryPropertyCollection()
            };
            var data = new SearchResultCollectionProxy(new List<SearchResultProxy>() { first });
            Mock.Get(Searcher.Base)
                .Setup(m => m.FindAll())
                .Returns(data);
            var results = Searcher.FindAll().ToList();
            Mock.Get(Creator).Verify(m => m.Create<TestLdapModel>(first.Properties, first.Path), Times.Once);
        }

        [WindowsOnlyFact]
        public void FindOne_ReturnsMappedFromBase() {
            var data = new SearchResultProxy()
            {
                Path = "ou=some,com=path",
                Properties = new DirectoryEntryPropertyCollection()
            };
            Mock.Get(Searcher.Base)
                .Setup(m => m.FindOne())
                .Returns(data);
            var results = Searcher.FindOne();
            Mock.Get(Creator).Verify(m => m.Create<TestLdapModel>(data.Properties, data.Path), Times.Once);
        }

        [WindowsOnlyFact]
        public void Filter_GetParses() {
            Searcher.RawFilter = "(cn=something)";
            Expression<Func<TestLdapModel, bool>> result = (m) => m["cn"] == "something";
            Assert.Equal(result.ToString(), Searcher.Filter.ToString());
        }

        [WindowsOnlyFact]
        public void Filter_SetCompiles() {
            Searcher.Filter = (m) => m.CommonName == "something";
            Assert.Equal("(cn=something)", Searcher.RawFilter);
        }

        [WindowsOnlyFact]
        public void RawFilter_SetsWithoutError() {
            var val = "(cn=one)";
            Searcher.RawFilter = val;
            Mock.Get(Searcher.Base).VerifySet(m => m.Filter = val, Times.Once);
        }

        [WindowsOnlyFact]
        public void RawFilter_GetsWithoutError()
        {
            var val = Searcher.RawFilter;
            Mock.Get(Searcher.Base).VerifyGet(m => m.Filter, Times.Once);
        }
    }
}