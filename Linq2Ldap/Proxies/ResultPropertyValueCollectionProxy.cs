using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using Linq2Ldap.Types;

namespace Linq2Ldap.Proxies
{
    public class ResultPropertyValueCollectionProxy
        : IEnumerable<object>, IEquatable<string>, IManyComparable<string>
    {
        protected ResultPropertyValueCollection ProxyValues { get; }
        protected List<object> Values { get; }
        public ResultPropertyValueCollectionProxy(ResultPropertyValueCollection proxyValues)
        {
            if (proxyValues == null) {
                    throw new ArgumentException("Cannot be null.", nameof(proxyValues));
            }

            ProxyValues = proxyValues;
        }

        public ResultPropertyValueCollectionProxy(List<object> mockValues)
        {
            if (mockValues == null) {
                    throw new ArgumentException("Cannot be null.", nameof(mockValues));
            }

            Values = mockValues;
        }

        public int Count => Values?.Count ?? ProxyValues?.Count ?? 0;

        public object this[int index]
        {
            get
            {
                return Values != null
                    ? Values[index]
                    : ProxyValues[index];
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        public override int GetHashCode()
        {
            return Values != null ? Values.GetHashCode() : ProxyValues.GetHashCode();
        }

        public static bool operator ==(ResultPropertyValueCollectionProxy a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) == 0)
                ?? b == null;

        public static bool operator !=(ResultPropertyValueCollectionProxy a, string b)
            => !(a == b);

        public static bool operator <(ResultPropertyValueCollectionProxy a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) < 0)
                ?? throw new ArgumentException("Arguments to < cannot be null.");

        public static bool operator >(ResultPropertyValueCollectionProxy a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) > 0)
                ?? throw new ArgumentException("Arguments to > cannot be null.");

        public static bool operator <=(ResultPropertyValueCollectionProxy a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) <= 0)
                ?? throw new ArgumentException("Arguments to <= cannot be null.");

        public static bool operator >=(ResultPropertyValueCollectionProxy a, string b)
            => a?.Any(m => string.CompareOrdinal(m.ToString(), b) >= 0)
                ?? throw new ArgumentException("Arguments to >= cannot be null.");

        public static implicit operator ResultPropertyValueCollectionProxy(string[] list)
            => new ResultPropertyValueCollectionProxy(new List<object>(list));

        public bool StartsWith(string frag) => this.Any(s => s.ToString().StartsWith(frag));
        public bool EndsWith(string frag) => this.Any(s => s.ToString().EndsWith(frag));
        public bool Contains(string frag) => this.Any(s => s.ToString().Contains(frag));

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
