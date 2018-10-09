using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Attributes
{
    public class LDAPFieldAttribute: Attribute
    {
        public string Name { get; set; }

        public LDAPFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
