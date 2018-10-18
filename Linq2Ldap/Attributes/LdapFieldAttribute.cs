using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Attributes
{
    public class LDAPFieldAttribute: Attribute
    {
        public string Name { get; set; }
        public bool Optional { get; set; }

        public LDAPFieldAttribute(string name, bool optional = false)
        {
            Name = name;
            Optional = optional;
        }
    }
}
