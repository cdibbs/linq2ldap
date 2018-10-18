using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Types;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class TestLdapModel: Entry
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

        [LDAPField("id")]
        public LDAPInt Id { get; set; }

        [LDAPField("alt-emails")]
        public LDAPStringList AltEmails { get; set; }
    }
}
