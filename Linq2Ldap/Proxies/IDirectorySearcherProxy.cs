using System.Collections.Specialized;
using System.DirectoryServices;

namespace Linq2Ldap.Proxies
{
    public interface IDirectorySearcherProxy
    {
        SearchResultCollection FindAll();
        SearchResult FindOne();
        string Filter { get; set; }
        int PageSize { get; set; }
        StringCollection PropertiesToLoad { get; }
        void Dispose();
    }
}