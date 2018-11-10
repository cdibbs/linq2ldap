using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Core.Attributes
{
    public class LdapFieldAttribute: Attribute
    {
        public string Name { get; set; }
        public bool Optional { get; set; }

        public LdapFieldAttribute(string name, bool optional = false)
        {
            Name = name;
            Optional = optional;
        }
    }
}
