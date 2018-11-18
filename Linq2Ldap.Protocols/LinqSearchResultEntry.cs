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
        public virtual dynamic Native { get; set; }
        public virtual DirectoryControl[] Controls { get; set; }
    }
}
