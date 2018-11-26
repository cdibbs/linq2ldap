using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Proxies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Text;

namespace Linq2Ldap.Protocols
{
    /// <summary>
    /// A wrapper around a Linq2Ldap.Core.Models.Entry that adds properties
    /// to access the native Protocols entry object.
    /// </summary>
    public class LinqSearchResultEntry : Entry
    {
        /// <summary>
        /// The native search result entry.
        /// </summary>
        public virtual dynamic Native { get; set; }

        /// <summary>
        /// The native directory controls array.
        /// </summary>
        public virtual DirectoryControl[] Controls { get; set; }
    }
}
