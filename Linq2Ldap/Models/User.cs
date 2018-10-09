using Linq2Ldap.Attributes;

namespace Linq2Ldap.Models
{
    public class User: BaseSAMAccount
    {
        public string Url { get; set; }

        [LDAPField("mail")]
        public string Email { get; set; }

        [LDAPField("givenname")]
        public string GivenName { get; set; }

        [LDAPField("sn")]
        public string Surname { get; set; }

        [LDAPField("samaccountname")]
        public string UserId { get; set; }
    }
}
