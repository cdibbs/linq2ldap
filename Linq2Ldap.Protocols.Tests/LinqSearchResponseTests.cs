using Linq2Ldap.Core.Proxies;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Xunit;

namespace Linq2Ldap.Protocols.Tests
{
    public class LinqSearchResponseTests
    {
        [Fact]
        public void Entries_IteratesNativeAndReturnsWrapped()
        {
            var pseudoNative = (SearchResponse) FormatterServices.GetUninitializedObject(typeof(SearchResponse));
            var pseudoEntries = new List<FakeSearchResultEntry>()
            {
                new FakeSearchResultEntry()
                {
                    DistinguishedName = "awesome",
                    Attributes = new DirectoryEntryPropertyCollection(),
                    Controls = new DirectoryControl[0]
                }
            };
            var resp = new LinqSearchResponse<LinqSearchResultEntry>(pseudoNative, pseudoEntries);

            var entry = resp.Entries.ToList().FirstOrDefault();
            Assert.NotNull(entry);
            Assert.IsType<LinqSearchResultEntry>(entry);
            Assert.Equal(pseudoEntries.First(), entry.Native);
            Assert.Equal(pseudoEntries.First().DistinguishedName, entry.DistinguishedName);
            Assert.Equal(pseudoEntries.First().Attributes, entry.Attributes);
        }

        public class FakeSearchResultEntry // must have same attributes as inaccessible SearchResultEntry
        {
            public string DistinguishedName { get; set; }
            public DirectoryEntryPropertyCollection Attributes { get; set; }
            public DirectoryControl[] Controls { get; set; }
        }
    }
}
