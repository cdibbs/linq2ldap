using Linq2Ldap.Core.Types;

namespace Linq2Ldap.Core.Types {
    public interface ILdapComparable<T> where T: System.IComparable
    {
        IntList CompareTo(T b);
    }
}