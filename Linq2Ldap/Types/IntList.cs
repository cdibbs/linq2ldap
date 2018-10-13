using System.Collections.Generic;
using System.Linq;

namespace Linq2Ldap.Types {

    /// <summary>
    /// Facilitates CompareTo on LDAPStringLists.
    /// </summary>
    public class IntList: List<int> {

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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}