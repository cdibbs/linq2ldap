using System.DirectoryServices.Protocols;

namespace Linq2Ldap.Protocols.Proxies
{
    public interface ILdapConnectionProxy
    {
        DirectoryResponse SendRequest(DirectoryRequest request);
    }
}