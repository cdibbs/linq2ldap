using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.Examples.Models
{
    public class User: Entry
    {
        public LdapString Url { get; set; }

        [LdapField("mail")]
        public string Email { get; set; }

        [LdapField("givenname")]
        public string GivenName { get; set; }

        [LdapField("sn")]
        public string Surname { get; set; }

        [LdapField("samaccountname")]
        public string SamAccountName { get; set; }

        [LdapField("cn")]
        public string CommonName { get; set; }

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
