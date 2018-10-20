using System.Collections.Generic;
using System.Linq;

namespace Linq2Ldap.Types {

    /// <summary>
    /// Facilitates CompareTo on BaseLDAPManyType and subclasses
    /// by providing comparison operator overloads on lists of
    /// integer CompareTo results. This accommodates LDAPs
    /// multi-valued fields. Ex: (mails=something*)
    /// </summary>
    public class IntList: List<int> {
        public IntList(): base() {}

        public IntList(IEnumerable<int> ints): base(ints) {}

        public static bool operator ==(IntList a, int b)
            => a?.Any(m => m == b) ?? false;

        public static bool operator !=(IntList a, int b)
            => !(a == b);

        public static bool operator <(IntList a, int b)
            => a?.Any(m => m < b) ?? false;

        public static bool operator >(IntList a, int b)
            => a?.Any(m => m > b) ?? false;

        public static bool operator <=(IntList a, int b)
            => a?.Any(m => m <= b) ?? false;

        public static bool operator >=(IntList a, int b)
            => a?.Any(m => m >= b) ?? false;
    }
}