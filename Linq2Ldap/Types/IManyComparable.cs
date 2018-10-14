using Linq2Ldap.Types;

public interface IManyComparable<T> where T: System.IComparable
{
    IntList CompareTo(T b);
}