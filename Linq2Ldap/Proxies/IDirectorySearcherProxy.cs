using System.Collections.Specialized;
using System.DirectoryServices;

namespace Linq2Ldap.Proxies
{
    public interface IDirectorySearcherProxy
    {
        SearchResultCollectionProxy FindAll();
        SearchResultProxy FindOne();
        string Filter { get; set; }
        int PageSize { get; set; }
        StringCollection PropertiesToLoad { get; }
        void Dispose();
    }
}