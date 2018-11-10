using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.Core.Proxies
{
    public class PropertyValueCollection
        : List<object>, IEquatable<string>, ILdapComparable<string>
    {
        public PropertyValueCollection(): base() {}
        public PropertyValueCollection(IEnumerable proxyValues): base(proxyValues.Cast<object>())
        {
        }

        public IntList CompareTo(string other)
        {
            var results = new IntList();
            foreach (var v in this) {
                results.Add(string.CompareOrdinal(v.ToString(), other));
            }
            return results;
        }

        public bool Equals(string other) => this == other;

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(PropertyValueCollection a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) == 0)
                ?? b == null;

        public static bool operator !=(PropertyValueCollection a, string b)
            => !(a == b);

        public static bool operator <(PropertyValueCollection a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) < 0)
                ?? throw new ArgumentException("Arguments to < cannot be null.");

        public static bool operator >(PropertyValueCollection a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) > 0)
                ?? throw new ArgumentException("Arguments to > cannot be null.");

        public static bool operator <=(PropertyValueCollection a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) <= 0)
                ?? throw new ArgumentException("Arguments to <= cannot be null.");

        public static bool operator >=(PropertyValueCollection a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) >= 0)
                ?? throw new ArgumentException("Arguments to >= cannot be null.");

        public static implicit operator PropertyValueCollection(string[] list)
            => new PropertyValueCollection(new List<object>(list));

        public static implicit operator PropertyValueCollection(ReadOnlyCollectionBase list)
            => new PropertyValueCollection(list);

        public bool StartsWith(string frag) => this.Any(s => s.ToString().StartsWith(frag));
        public bool EndsWith(string frag) => this.Any(s => s.ToString().EndsWith(frag));
        public bool Contains(string frag) => this.Any(s => s.ToString().Contains(frag));

        public override bool Equals(object obj) => base.Equals(obj);
    }
}
