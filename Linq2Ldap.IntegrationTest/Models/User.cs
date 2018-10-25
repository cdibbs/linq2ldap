using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Types;

namespace Linq2Ldap.IntegrationTest.Models {
    public class User: MyModel {
        [LdapField("samaccount")]
        public LdapInt Id { get; set; }

        [LdapField("status")]
        public string Status { get; set; }

        [LdapField("suspended")]
        public string Suspended { get; set; }

        [LdapField("country")]
        public string Country { get; set; }
    }
}