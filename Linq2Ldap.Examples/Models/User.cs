using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Types;

namespace Linq2Ldap.Examples.Models
{
    public class User: Entry
    {
        public LDAPString Url { get; set; }

        [LDAPField("mail")]
        public string Email { get; set; }

        [LDAPField("givenname")]
        public string GivenName { get; set; }

        [LDAPField("sn")]
        public string Surname { get; set; }

        [LDAPField("samaccountname")]
        public string SamAccountName { get; set; }

        [LDAPField("cn")]
        public string CommonName { get; set; }
    }
}
