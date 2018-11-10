using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Core.Models
{
    public interface IEntry
    {
        string DistinguishedName { get; set; }

        DirectoryEntryPropertyCollection Attributes { get; set; }

        bool Has(string attrName);

        PropertyValueCollection this[string key] { get; }
        //ICollection Keys { get; }
        //ICollection Values { get; }
    }
}
