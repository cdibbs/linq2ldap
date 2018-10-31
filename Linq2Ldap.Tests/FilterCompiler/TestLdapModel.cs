using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Types;

namespace Linq2Ldap.Tests.FilterCompiler
{
    public class TestLdapModel: Entry
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

        [LdapField("id")]
        public LdapInt Id { get; set; }

        [LdapField("alt-emails")]
        public LdapStringList AltEmails { get; set; }

        [LdapField(" we ird  ")]
        public LdapString WeirdName { get; set; }
    }
}
