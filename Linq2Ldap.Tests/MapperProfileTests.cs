using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using AutoMapper;
using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Proxies;
using Linq2Ldap.Types;
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
                c.AddProfile<MapperProfile<MyTestModel>>();
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
                    { "dn", new string[]{ "ou=some, ou=dn" } },
                    { "cn", new string[] { "example" } },
                    { "objectclass", new string[] { "testuser" } },
                    { "objectsid", new ResultPropertyValueCollectionProxy(new List<object> { new byte[] { 0x31, 0x41} }) },
                    { "userprincipalname", new string[] { "testuser" } },
                    { "samaccountname", new string[] { "testuser" } },
                    { "mail", new string[] { "anemail@example.com" } },
                    { "alt-mails", new string[] { "anemail@example.com", "mail2@example.com", "mail3@example.com" } }
                }
            );
            var b = Mapper.Map<MyTestModel>(srp);
            Assert.Equal(srp.Properties, b.Properties);
            //Assert.Equal(srp.Properties["objectsid"][0], b.ObjectSid);
            //Assert.Equal(srp.Properties["userprincipalname"][0], b.UserPrincipalName);
            //Assert.Equal(srp.Properties["samaccountname"][0], b.SamAccountName);
        }
    }

    public class MyTestModel: Entry {

        [LDAPField("mail")]
        public string Mail { get; set; }

        [LDAPField("alt-mails")]
        public LDAPStringList AltMails { get; set; }
    }
}
