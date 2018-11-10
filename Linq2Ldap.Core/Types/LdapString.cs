using System;
using System.Collections.Generic;
using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Core.Types
{
    public class LdapString: BaseLdapType<string>
    {
        public LdapString(PropertyValueCollection raw): base(raw)
        {
        }

        public static implicit operator string(LdapString i) => i.GetString();
        public static implicit operator LdapString(string i)
            => new LdapString(new PropertyValueCollection(new List<object> { i }));

        protected string GetString() {
            if (Raw == null || Raw.Count == 0) {
                throw new InvalidOperationException("LdapInt value access from empty property bag.");
            }

            if (this.Raw[0].GetType() == typeof(string)) {
                return (string)this.Raw[0];
            }
            return this.Raw[0].ToString();
        }

        // We'll choose not to make a public version of this for ints, because what would that mean for empty bags?
        protected override int _CompareTo(object b) => GetString().CompareTo(b);

        public override string ToString() => GetString();
    }
}