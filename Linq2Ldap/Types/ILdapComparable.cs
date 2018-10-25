using Linq2Ldap.Types;

public interface ILdapComparable<T> where T: System.IComparable
{
    IntList CompareTo(T b);
}