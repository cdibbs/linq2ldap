
namespace Linq2Ldap.Core.Types {
    public interface IAttribute {
        bool StartsWith(string frag);
        bool EndsWith(string frag);
        bool Contains(string frag);
        IntList CompareTo(string b);

        bool Equals(object obj);
    }
}