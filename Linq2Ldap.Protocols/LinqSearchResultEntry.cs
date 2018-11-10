using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Proxies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Text;

namespace Linq2Ldap.Protocols
{
    public class LinqSearchResultEntry : Entry
    {
        public LinqSearchResultEntry(string dn, IDictionary properties, DirectoryControl[] controls, SearchResultEntry native)
        {
            DistinguishedName = dn;
            Controls = controls;
            Native = native;
            Attributes = new DirectoryEntryPropertyCollection();
            foreach (var k in properties.Keys)
            {
                if (properties[k] is IEnumerable e)
                {
                    Attributes.Add(k.ToString(), new PropertyValueCollection(e));
                } else
                {
                    throw new ArgumentException($"Directory entry at key {k} not an IEnumerable.");
                }
            }
        }

        public virtual SearchResultEntry Native { get; set; }

        public virtual DirectoryControl[] Controls { get; set; }
    }
}
