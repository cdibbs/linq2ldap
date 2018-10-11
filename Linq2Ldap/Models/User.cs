using Linq2Ldap.Attributes;
using Linq2Ldap.Types;

namespace Linq2Ldap.Models
{
    public class User: BaseEntry
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
