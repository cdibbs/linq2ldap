using Linq2Ldap.Core.Proxies;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Linq2Ldap.Core;
using Linq2Ldap.Core.Attributes;
using Moq;
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
                    Attributes = new EntryAttributeDictionary()
                    {
                        { "cn", new AttributeValueList("whoa") }
                    },
                    Controls = new DirectoryControl[0]
                }
            };
            var resp = new LinqSearchResponse<CustomEntry>(pseudoNative, pseudoEntries);

            var entry = resp.Entries.ToList().FirstOrDefault();
            Assert.NotNull(entry);
            Assert.IsType<CustomEntry>(entry);
            Assert.Equal(pseudoEntries.First(), entry.Native);
            Assert.Equal(pseudoEntries.First().DistinguishedName, entry.DistinguishedName);
            Assert.Equal(pseudoEntries.First().Attributes, entry.Attributes);
            Assert.Equal(pseudoEntries.First().Attributes["cn"][0], entry.CommonName);
        }

        public class FakeSearchResultEntry // must have same attributes as inaccessible SearchResultEntry
        {
            public string DistinguishedName { get; set; }
            public EntryAttributeDictionary Attributes { get; set; }
            public DirectoryControl[] Controls { get; set; }
        }

        public class CustomEntry : LinqSearchResultEntry
        {
            [LdapField("cn")]
            public string CommonName { get; set; }
        }
    }
}
