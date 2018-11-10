using System.Collections.Generic;
using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.Core.Types;
using Xunit;

namespace Linq2Ldap.Core.Tests
{
    public class ModelCreatorTests
    {
        private ModelCreator Creator;
        public ModelCreatorTests()
        {
            Creator = new ModelCreator();
        }

        [Fact]
        public void Create_ResultProperties_To_Model()
        {
            var path = "test path";
            var properties = new DirectoryEntryPropertyCollection(
                new Dictionary<string, Core.Proxies.PropertyValueCollection>()
                {
                    { "dn", new string[]{ "ou=some, ou=dn" } },
                    { "cn", new string[] { "example" } },
                    { "objectclass", new string[] { "testuser" } },
                    { "objectsid", new Core.Proxies.PropertyValueCollection(new List<object> { new byte[] { 0x31, 0x41} }) },
                    { "userprincipalname", new string[] { "testuser" } },
                    { "samaccountname", new string[] { "testuser" } },
                    { "mail", new string[] { "anemail@example.com" } },
                    { "alt-mails", new string[] { "anemail@example.com", "mail2@example.com", "mail3@example.com" } }
                }
            );
            var b = Creator.Create<MyTestModel>(properties, path);
            Assert.Equal(properties, b.Attributes);
            Assert.Equal(path, b.DistinguishedName);
        }

        [Fact]
        public void NullProp_YieldsNullField() {
            var path = "test path";
            var properties = new DirectoryEntryPropertyCollection(
                new Dictionary<string, Core.Proxies.PropertyValueCollection>()
                {
                    // { "mail", new string[] { "anemail@example.com" } },
                    { "alt-mails", new string[] { "anemail@example.com", "mail2@example.com", "mail3@example.com" } }
                }
            );
            var b = Creator.Create<MyTestModel>(properties, path);
            Assert.Null(b.Mail);
        }

        [Fact]
        public void NullManyProp_Optional_YieldsNullField() {
            var path = "test path";
            var properties = new DirectoryEntryPropertyCollection(
                new Dictionary<string, Core.Proxies.PropertyValueCollection>()
                {
                    { "mail", new string[] { "anemail@example.com" } },
                    // { "alt-mails", new string[] { "anemail@example.com", "mail2@example.com", "mail3@example.com" } }
                }
            );
            var b = Creator.Create<MyTestModel>(properties, path);
            Assert.Null(b.AltMails);
        }

        [Fact]
        public void EmptyProp_Optional_YieldsNullField() {
            var path = "test path";
            var properties = new DirectoryEntryPropertyCollection(
                new Dictionary<string, Core.Proxies.PropertyValueCollection>()
                {
                    { "mail", new string[] { } },
                }
            );
            var b = Creator.Create<MyTestModel>(properties, path);
            Assert.Null(b.Mail);
        }

        [Fact]
        public void EmptyManyProp_Optional_YieldsEmptyField() {
            var path = "test path";
            var properties = new DirectoryEntryPropertyCollection(
                new Dictionary<string, Core.Proxies.PropertyValueCollection>()
                {
                    { "alt-mails", new string[] { } }
                }
            );
            var b = Creator.Create<MyTestModel>(properties, path);
            Assert.Empty(b.AltMails);
        }
    }

    public class MyTestModel: Entry {

        [LdapField("mail", optional: true)]
        public string Mail { get; set; }

        [LdapField("alt-mails", optional: true)]
        public LdapStringList AltMails { get; set; }
    }
}
