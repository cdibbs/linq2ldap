using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Types
{
    public abstract class BaseLDAPManyType<T, TConv>: List<T>, IManyComparable<T>
        where T: IComparable
        where TConv: class, IConverter<List<T>>
    {
        protected ResultPropertyValueCollectionProxy Raw { get; set; }
        public BaseLDAPManyType(ResultPropertyValueCollectionProxy raw, TConv conv)
            : base(conv.Convert(raw))
        {
            this.Raw = raw;               
        }

        public static bool operator ==(BaseLDAPManyType<T, TConv> a, string b)
            => a?.Any(m => m.CompareTo(b) == 0) ?? b == null;

        public static bool operator !=(BaseLDAPManyType<T, TConv> a, string b)
            => !(a == b);

        public static bool operator <(BaseLDAPManyType<T, TConv> a, string b)
            => a?.Any(m => m.CompareTo(b) < 0) ?? b == null;

        public static bool operator >(BaseLDAPManyType<T, TConv> a, string b)
            => a?.Any(m => m.CompareTo(b) > 0) ?? b == null;

        public static bool operator <=(BaseLDAPManyType<T, TConv> a, string b)
            => a?.Any(m => m.CompareTo(b) <= 0) ?? b == null;

        public static bool operator >=(BaseLDAPManyType<T, TConv> a, string b)
            => a?.Any(m => m.CompareTo(b) >= 0) ?? b == null;

        public virtual bool StartsWith(string frag) => this.Raw.Any(s => s.ToString().StartsWith(frag));
        public virtual bool EndsWith(string frag) => this.Raw.Any(s => s.ToString().EndsWith(frag));
        public virtual bool Contains(string frag) => this.Raw.Any(s => s.ToString().Contains(frag));

        /// <summary>
        /// Compares a multi-valued LDAP list with the given string.
        /// Warning: by necessity, this is a little quirky (look at return value)
        /// due to the use of implicit operators. Serialization should still work
        /// fine, though.
        /// </summary>
        /// <param name="b">The string to compare with.</param>
        /// <returns>An IntList of individual CompareTo results.</returns>
        public IntList CompareTo(T b) => new IntList(this.Select(s => s.CompareTo(b)));
    }
}