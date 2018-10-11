using Linq2Ldap.Attributes;
using Linq2Ldap.Types;

namespace Linq2Ldap.Models
{
    public class BaseEntry: IEntry
    {
        [LDAPIgnore]
        public ResultPropertyCollectionProxy Properties { get; set; }
        public string DN { get; set; }

        public virtual bool Has(string attrName) => this.Properties.Contains(attrName);
    }
}
