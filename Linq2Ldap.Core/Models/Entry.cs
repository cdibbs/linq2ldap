using System.Collections;
using System.Collections.Generic;
using Linq2Ldap.Core.Attributes;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.Core.Models
{
    public class Entry: IEntry
    {
        public string DistinguishedName { get; set; }

        public DirectoryEntryPropertyCollection Attributes { get; set; }

        public virtual bool Has(string attrName) => this.Attributes.ContainsKey(attrName);

        public ICollection Keys => this.Attributes.Keys;
        public ICollection Values => this.Attributes.Values;

        public PropertyValueCollection this[string key] {
            get => this.Attributes.ContainsKey(key) ? this.Attributes[key] : null;
        }
    }
}
