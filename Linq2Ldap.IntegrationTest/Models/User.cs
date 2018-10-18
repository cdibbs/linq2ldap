using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Types;

namespace Linq2Ldap.IntegrationTest.Models {
    public class User: MyModel {
        [LDAPField("samaccount")]
        public LDAPInt Id { get; set; }

        [LDAPField("status")]
        public string Status { get; set; }

        [LDAPField("suspended")]
        public string Suspended { get; set; }

        [LDAPField("country")]
        public string Country { get; set; }
    }
}