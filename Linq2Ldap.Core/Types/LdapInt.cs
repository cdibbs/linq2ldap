using System;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.Core.Types;
using Linq2Ldap.Core.ExtensionMethods;
using System.Collections.Generic;

namespace Linq2Ldap.Core.Types {
    public class LdapInt : BaseLdapType<int>
    {
        public LdapInt(PropertyValueCollection raw) : base(raw)
        {
        }

        public static implicit operator int(LdapInt i) => i.GetInt();
        public static implicit operator LdapInt(int i) => new LdapInt(new PropertyValueCollection(new List<object> { i }));

        protected int GetInt() {
            if (Raw == null || Raw.Count == 0) {
                throw new InvalidOperationException("LdapInt value access from empty property bag.");
            }

            if (this.Raw[0].GetType() == typeof(int)) {
                return (int)this.Raw[0];
            }
            return int.Parse(this.Raw[0].ToString());
        }

        // We'll choose not to make a public version of this for ints, because what would that mean for empty bags?
        protected override int _CompareTo(object b) => GetInt().CompareTo(b);
    }
}