using Linq2Ldap.Types;

public interface ILDAPComparable<T> where T: System.IComparable
{
    IntList CompareTo(T b);
}