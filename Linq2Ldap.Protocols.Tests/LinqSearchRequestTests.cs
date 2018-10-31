using System;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Protocols.Tests
{
    public class LinqSearchRequestTests
    {
        [Fact]
        public void Test1()
        {
            var inst = new LinqSearchRequest<Entry>();
            inst.Filter = (Entry e) => e["what"] == "asdf";
        }
    }
}
