using System;
using Linq2Ldap.Proxies;
using Linq2Ldap.Types;
using Linq2Ldap.ExtensionMethods;
using System.Collections.Generic;

namespace Linq2Ldap.Types {
    public class LDAPInt : BaseLDAPType<int>
    {
        public LDAPInt(ResultPropertyValueCollectionProxy raw) : base(raw)
        {
        }

        public static implicit operator int(LDAPInt i) => i.GetInt();
        public static implicit operator LDAPInt(int i) => new LDAPInt(new ResultPropertyValueCollectionProxy(new List<object> { i }));

        protected int GetInt() {
            if (Raw == null || Raw.Count == 0) {
                throw new InvalidOperationException("LDAPInt value access from empty property bag.");
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