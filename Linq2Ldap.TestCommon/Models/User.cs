using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.TestCommon.Models {
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