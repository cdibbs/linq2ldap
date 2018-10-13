using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Linq2Ldap.Proxies
{
    public class ResultPropertyValueCollectionProxy
        : IEnumerable<object>, IComparable<string>, IEquatable<string>
    {
        protected ResultPropertyValueCollection ProxyValues { get; }
        protected List<object> Values { get; }
        public ResultPropertyValueCollectionProxy(ResultPropertyValueCollection proxyValues)
        {
            ProxyValues = proxyValues;
        }

        public ResultPropertyValueCollectionProxy(List<object> values)
        {
            Values = values;
        }

        public int Count => Values?.Count ?? ProxyValues.Count;

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

        public int CompareTo(string other)
        {
            return Count < 1 ? -1 : string.CompareOrdinal(this[0] as string, other);
        }

        public bool Equals(string other) => this == other;

        public override int GetHashCode()
        {
            return Values != null ? Values.GetHashCode() : ProxyValues.GetHashCode();
        }

        public static bool operator ==(ResultPropertyValueCollectionProxy a, string b)
            => a != null && (a.Count > 0 && a[0] as string == b);

        public static bool operator !=(ResultPropertyValueCollectionProxy a, string b)
            => !(a == b);

        public static bool operator <(ResultPropertyValueCollectionProxy a, string b)
            => a.Count < 1 || string.CompareOrdinal(a[0] as string, b) < 0;

        public static bool operator >=(ResultPropertyValueCollectionProxy a, string b)
            => !(a < b);

        public static bool operator >(ResultPropertyValueCollectionProxy a, string b)
            => !(a.Count < 1) && string.CompareOrdinal(a[0] as string, b) > 0;

        public static bool operator <=(ResultPropertyValueCollectionProxy a, string b)
            => !(a > b);

        public bool StartsWith(string frag) => throw new NotImplementedException("This helper method exists only to facilitate Linq2Ldap Expressions.");
        public bool EndsWith(string frag) => throw new NotImplementedException("This helper method exists only to facilitate Linq2Ldap Expressions.");
        public bool Contains(string frag) => throw new NotImplementedException("This helper method exists only to facilitate Linq2Ldap Expressions.");

        

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
