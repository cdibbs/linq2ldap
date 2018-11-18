using Moq;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.Serialization;
using Xunit;

namespace Linq2Ldap.Protocols.Tests
{
    public class Linq2LdapConnectionTests
    {
        [Fact]
        public void CtorString_SendRequest_SendsThroughProxy_WrapsProxyResponse()
        {
            var conn = new Linq2LdapConnection("bogus");
            AssertSendRequestWraps(conn);
        }

        [Fact]
        public void CtorDirectoryId_SendRequest_SendsThroughProxy_WrapsProxyResponse()
        {
            var conn = new Linq2LdapConnection(new LdapDirectoryIdentifier("bogus"));
            AssertSendRequestWraps(conn);
        }

        [Fact]
        public void CtorNetCred_SendRequest_SendsThroughProxy_WrapsProxyResponse()
        {
            var conn = new Linq2LdapConnection(new LdapDirectoryIdentifier("bogus"), new NetworkCredential());
            AssertSendRequestWraps(conn);
        }

        [Fact]
        public void CtorAuthType_SendRequest_SendsThroughProxy_WrapsProxyResponse()
        {
            var conn = new Linq2LdapConnection(new LdapDirectoryIdentifier("bogus"), new NetworkCredential(), AuthType.Anonymous);
            AssertSendRequestWraps(conn);
        }

        protected void AssertSendRequestWraps(Linq2LdapConnection conn)
        {
            var search = new LinqSearchRequest<LinqSearchResultEntry>();
            var testResponse = (SearchResponse)FormatterServices.GetUninitializedObject(typeof(SearchResponse));
            conn.LdapConnectionProxy = Mock.Of<ILdapConnectionProxy>(
                c => c.SendRequest(search) == testResponse);
            var result = conn.SendRequest(search);

            Mock.Get(conn.LdapConnectionProxy)
                .Verify(c => c.SendRequest(search), Times.Once());
            Assert.Equal(testResponse, result.Native);
        }
    }
}
