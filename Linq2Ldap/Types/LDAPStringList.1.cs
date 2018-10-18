using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.ExtensionMethods;

namespace Linq2Ldap.Types
{
    /*
    public class LDAPStringListOld: List<string>, IAttribute, IManyComparable<string>
    {
        public LDAPStringListOld(IEnumerable<string> e) : base(e)
        {
        }

        public static bool operator ==(LDAPStringList a, string b)
            => a?.Any(m => string.CompareOrdinal(m, b) == 0) ?? b == null;

        public static bool operator !=(LDAPStringList a, string b)
            => !(a == b);

        public static bool operator <(LDAPStringList a, string b)
            => a?.Any(m => string.CompareOrdinal(m, b) < 0) ?? b == null;

        public static bool operator >(LDAPStringList a, string b)
            => a?.Any(m => string.CompareOrdinal(m, b) > 0) ?? b == null;

        public static bool operator <=(LDAPStringList a, string b)
            => a?.Any(m => string.CompareOrdinal(m, b) <= 0) ?? b == null;

        public static bool operator >=(LDAPStringList a, string b)
            => a?.Any(m => string.CompareOrdinal(m, b) >= 0) ?? b == null;

        public static implicit operator LDAPStringList(string[] list)
            => new LDAPStringList(list);

        public bool StartsWith(string frag) => this.Any(s => s.StartsWith(frag));
        public bool EndsWith(string frag) => this.Any(s => s.EndsWith(frag));
        public new bool Contains(string frag) => this.Any(s => s.Contains(frag));

        /// <summary>
        /// Compares a multi-valued LDAP list with the given string.
        /// Warning: by necessity, this is a little quirky (look at return value)
        /// due to the use of implicit operators. Serialization should still work
        /// fine, though.
        /// </summary>
        /// <param name="b">The string to compare with.</param>
        /// <returns>An IntList of individual CompareTo results.</returns>
        public IntList CompareTo(string b) => new IntList(this.Select(s => s.CompareTo(b)));

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
    }
    */
}
