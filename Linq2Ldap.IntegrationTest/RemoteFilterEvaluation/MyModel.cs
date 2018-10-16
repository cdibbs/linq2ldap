using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Types;

namespace Linq2Ldap.IntegrationTest.RemoteFilterEvaluation {
    public class MyModel: Entry {

        [LDAPField("mail")]
        public string Mail { get; set; }

        [LDAPField("alt-mails")]
        public LDAPStringList AltMails { get; set; }
    }
}