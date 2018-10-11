using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Models
{
    public interface IEntry
    {
        string DN { get; set; }

        ResultPropertyCollectionProxy Properties { get; set; }

        bool Has(string attrName);
    }
}
