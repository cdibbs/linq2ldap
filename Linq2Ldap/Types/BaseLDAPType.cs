using System;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Types
{
    public abstract class BaseLDAPType<T>: ILDAPComparable<T>
        where T: IComparable
    {
        internal protected ResultPropertyValueCollectionProxy Raw { get; set; }
        public BaseLDAPType(ResultPropertyValueCollectionProxy raw) 
        {
            this.Raw = raw;
               
        }

        public static bool operator ==(BaseLDAPType<T> a, T b)
            => a == null ? b == null : a.Raw.Count > 0 && a._CompareTo(b) == 0;

        public static bool operator !=(BaseLDAPType<T> a, T b)
            => a == null ? b != null : a.Raw.Count == 0 || a._CompareTo(b) != 0;

        public static bool operator <(BaseLDAPType<T> a, T b)
            => a == null ? false : a.Raw.Count > 0 && a._CompareTo(b) < 0;

        public static bool operator >(BaseLDAPType<T> a, T b)
            => a == null ? false : a.Raw.Count > 0 && a._CompareTo(b) > 0;

        public static bool operator <=(BaseLDAPType<T> a, T b)
            => a == null ? false : a.Raw.Count > 0 && a._CompareTo(b) <= 0;

        public static bool operator >=(BaseLDAPType<T> a, T b)
            => a == null ? false : a.Raw.Count > 0 && a._CompareTo(b) >= 0;
        public static implicit operator string(BaseLDAPType<T> source)
            => source.ToString();

        public virtual bool StartsWith(string frag) => Raw.Count > 0 && this.ToString().StartsWith(frag);
        public virtual bool EndsWith(string frag) => Raw.Count > 0 && this.ToString().EndsWith(frag);
        public virtual bool Contains(string frag) => Raw.Count > 0 && this.ToString().Contains(frag);

        /* We need this, internally, but it can be misleading to have it public in the case of certain types
         * like ints. Thus, we'll leave it up to the sub-classers whether they want to implement IComparable
         * and add a public wrapper for their protected _CompareTo.
         */
        protected abstract int _CompareTo(object b);

        public IntList CompareTo(T b) => _CompareTo(b);

        public override string ToString() {
            return Raw.Count > 0 ? Raw[0].ToString() : null;
        }
    }
}