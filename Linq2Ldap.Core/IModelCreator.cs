using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Core {
    public interface IModelCreator {
        T Create<T>(DirectoryEntryPropertyCollection entryProps, string dn = null)
            where T: IEntry, new();
    }
}