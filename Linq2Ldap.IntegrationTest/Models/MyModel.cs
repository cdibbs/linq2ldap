using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.IntegrationTest.Models {
    public class MyModel: Entry {

        [LdapField("mail")]
        public string Mail { get; set; }

        [LdapField("alt-mails", optional: true)]
        public LdapStringList AltMails { get; set; }
    }
}