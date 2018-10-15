using Linq2Ldap.Attributes;
using Linq2Ldap.Models;

namespace Linq2Ldap.IntegrationTest.RemoteFilterEvaluation {
    public class MyModel: Entry {

        [LDAPField("mail")]
        public object Mail { get; set; }
    }
}