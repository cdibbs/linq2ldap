using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Types
{
    public class LdapStringList : BaseLdapManyType<string, StringListConverter>
    {
        public LdapStringList(ResultPropertyValueCollectionProxy raw)
            : this(raw, new StringListConverter())
        {
        }

        public LdapStringList(ResultPropertyValueCollectionProxy raw, StringListConverter conv)
            : base(raw, conv)
        {
        }

        public static implicit operator LdapStringList(string[] list)
            => new LdapStringList(list);
    }
}
