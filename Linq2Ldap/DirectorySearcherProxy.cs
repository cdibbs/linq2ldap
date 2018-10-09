using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Text;

namespace Linq2Ldap
{
    public class DirectorySearcherProxy: DirectorySearcher, IDirectorySearcherProxy
    {
        public DirectorySearcherProxy(DirectoryEntry entry) : base(entry)
        {
        }
    }
}
