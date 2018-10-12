using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using AutoMapper;
using Linq2Ldap.Models;
using Xunit;

namespace Linq2Ldap.Tests
{
    public class MapperProfileTests
    {
        private IMapper Mapper;
        public MapperProfileTests()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<Linq2Ldap.MapperProfile>();
            });
            Mapper = config.CreateMapper();
        }

        [Fact]
        public void Map_SearchResult_To_BaseSAMAccount()
        {
            var srp = new SearchResultProxy();
            srp.Path = "test path";
            srp.Properties = new ResultPropertyCollectionProxy(
                new Dictionary<string, ResultPropertyValueCollectionProxy>()
                {
                    { "dn", new ResultPropertyValueCollectionProxy(new List<object>{ "ou=some, ou=dn" }) },
                    { "cn", new ResultPropertyValueCollectionProxy(new List<object> { "example" }) },
                    { "objectclass", new ResultPropertyValueCollectionProxy(new List<object> { "testuser" }) },
                    { "objectsid", new ResultPropertyValueCollectionProxy(new List<object> { new byte[] { 0x31, 0x41} }) },
                    { "userprincipalname", new ResultPropertyValueCollectionProxy(new List<object> { "testuser" }) },
                    { "samaccountname", new ResultPropertyValueCollectionProxy(new List<object> { "testuser" }) }
                }
            );
            var b = Mapper.Map<BaseEntry>(srp);
            Assert.Equal(srp.Properties, b.Properties);
            //Assert.Equal(srp.Properties["objectsid"][0], b.ObjectSid);
            //Assert.Equal(srp.Properties["userprincipalname"][0], b.UserPrincipalName);
            //Assert.Equal(srp.Properties["samaccountname"][0], b.SamAccountName);
        }
    }
}
