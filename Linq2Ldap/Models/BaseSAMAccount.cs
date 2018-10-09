using Linq2Ldap.Attributes;
using Linq2Ldap.Types;

namespace Linq2Ldap.Models
{
    public class BaseSAMAccount: ISAMAccount
    {
        [LDAPIgnore]
        public string Path { get; set; }

        [LDAPIgnore]
        public string Id
        {
            get => SamAccountName;
            set => SamAccountName = value;
        }

        [LDAPField("objectclass")]
        public LDAPStringList ObjectClass { get; set; }

        /*[Column("objectsid")]
        public byte[] ObjectSid { get; set; }*/

        [LDAPField("cn")]
        public LDAPStringList CommonName { get; set; }

        [LDAPField("userprincipalname")]
        public string UserPrincipalName { get; set; }

        [LDAPField("samaccountname")]
        public string SamAccountName { get; set; }

        [LDAPIgnore]
        public long[] SIdHistory { get; set; }

        [LDAPIgnore]
        public ResultPropertyCollectionProxy Properties { get; set; }
    }
}
