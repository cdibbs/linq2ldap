﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq2Ldap.Types
{
    public class LDAPStringList: List<string>, IAttribute
    {
        public LDAPStringList(IEnumerable<string> e) : base(e)
        {
        }

        public static bool operator ==(LDAPStringList a, string b)
            => a != null && (a.Count > 0 && a[0] as string == b);

        public static bool operator !=(LDAPStringList a, string b)
            => !(a == b);

        public static bool operator <(LDAPStringList a, string b)
            => a.Any(m => string.CompareOrdinal(m, b) < 0);

        public static bool operator >=(LDAPStringList a, string b)
            => !(a < b);

        public static bool operator >(LDAPStringList a, string b)
            => a.Any(m => string.CompareOrdinal(m, b) > 0);

        public static bool operator <=(LDAPStringList a, string b)
            => !(a > b);

        public static implicit operator string(LDAPStringList list)
        {
            throw new NotImplementedException(
                "This is a helper method that exists only to facilitate the specification of LDAP filters"
                + " as Linq Expressions and cannot be directly invoked.");
        }

        public bool StartsWith(string frag) => this.Any(s => s.StartsWith(frag));
        public bool EndsWith(string frag) => this.Any(s => s.EndsWith(frag));
        public new bool Contains(string frag) => this.Any(s => s.Contains(frag));
        public int CompareTo(string b) => throw new NotImplementedException("This helper method exists only to facilitate Linq2Ldap Expressions.");

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