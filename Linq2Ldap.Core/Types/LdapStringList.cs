using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.Core.ExtensionMethods;
using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Core.Types
{
    public class LdapStringList : BaseLdapManyType<string, StringListConverter>
    {
        public LdapStringList(PropertyValueCollection raw)
            : this(raw, new StringListConverter())
        {
        }

        public LdapStringList(PropertyValueCollection raw, StringListConverter conv)
            : base(raw, conv)
        {
        }

        public static implicit operator LdapStringList(string[] list)
            => new LdapStringList(list);
    }
}
