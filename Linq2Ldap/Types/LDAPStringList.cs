using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Types
{
    public class LDAPStringList : BaseLDAPManyType<string, StringListConverter>
    {
        public LDAPStringList(ResultPropertyValueCollectionProxy raw)
            : this(raw, new StringListConverter())
        {
        }

        public LDAPStringList(ResultPropertyValueCollectionProxy raw, StringListConverter conv)
            : base(raw, conv)
        {
        }

        public static implicit operator LDAPStringList(string[] list)
            => new LDAPStringList(list);
    }
}
