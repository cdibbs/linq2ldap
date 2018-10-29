using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Models
{
    public interface IEntry
    {
        string DN { get; set; }

        ResultPropertyCollectionProxy Properties { get; set; }

        bool Has(string attrName);

        ResultPropertyValueCollectionProxy this[string key] { get; }
        ICollection Keys { get; }
        ICollection Values { get; }
    }
}
