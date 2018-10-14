using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Text;

namespace Linq2Ldap.Proxies
{
    public class DirectorySearcherProxy: DirectorySearcher, IDirectorySearcherProxy
    {
        public DirectorySearcherProxy(): base(){}
        public DirectorySearcherProxy(DirectoryEntry entry) : base(entry)
        {
        }

        public new SearchResultCollectionProxy FindAll() => base.FindAll();

        public new SearchResultProxy FindOne() => base.FindOne();
    }
}
