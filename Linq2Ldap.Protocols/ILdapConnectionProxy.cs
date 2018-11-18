using System.DirectoryServices.Protocols;

namespace Linq2Ldap.Protocols
{
    public interface ILdapConnectionProxy
    {
        DirectoryResponse SendRequest(DirectoryRequest request);
    }
}