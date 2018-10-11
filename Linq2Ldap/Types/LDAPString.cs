using System;

namespace Linq2Ldap.Types
{
    public class LDAPString
    {
        private string S;
        public LDAPString(string s)
        {
            S = s;
        }

        public static implicit operator string(LDAPString ls) => ls.ToString();

        public static implicit operator LDAPString(string s) => new LDAPString(s);
    }
}