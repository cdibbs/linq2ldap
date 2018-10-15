using System;
using System.Collections.Generic;
using System.DirectoryServices;
using AutoMapper;
using Linq2Ldap.Proxies;
using Linq2Ldap.Tests.FilterCompiler;
using Linq2Ldap.TestUtil;
using Moq;
using Xunit;

namespace Linq2Ldap.Tests
{
    public class LinqDirectorySearcherTests
    {
        IMapper Mapper { get; set; }
        LinqDirectorySearcher<TestLdapModel> Searcher;
        public LinqDirectorySearcherTests() {
            Mapper = Mock.Of<IMapper>();
            Searcher = new LinqDirectorySearcher<TestLdapModel>(Mapper);
            Searcher.Base = Mock.Of<IDirectorySearcherProxy>();
        }

        [WindowsOnlyFact]
        public void FindAll_ReturnsMappedFromBase() {
            var data = new SearchResultCollectionProxy(new List<SearchResultProxy>());
            Mock.Get(Searcher.Base)
                .Setup(m => m.FindAll())
                .Returns(data);
            var results = Searcher.FindAll();
            Mock.Get(Mapper).Verify(m => m.Map<IEnumerable<TestLdapModel>>(data), Times.Once);
        }

        [WindowsOnlyFact]
        public void FindOne_ReturnsMappedFromBase() {
            var data = new SearchResultProxy();
            Mock.Get(Searcher.Base)
                .Setup(m => m.FindOne())
                .Returns(data);
            var results = Searcher.FindOne();
            Mock.Get(Mapper).Verify(m => m.Map<TestLdapModel>(data), Times.Once);
        }

        [WindowsOnlyFact]
        public void Filter_GetThrows() {
            Assert.Throws<NotImplementedException>(() => Searcher.Filter);
        }

        [WindowsOnlyFact]
        public void Filter_SetCompiles() {
            Searcher.Filter = (m) => m.CommonName == "something";
            Assert.Equal(Searcher.RawFilter, "(cn=something)");
        }

        [WindowsOnlyFact]
        public void RawFilter_SetsWithoutError() {
            var val = "(cn=one)";
            Searcher.RawFilter = val;
            Mock.Get(Searcher.Base).VerifySet(m => m.Filter = val, Times.Once);
        }
    }
}