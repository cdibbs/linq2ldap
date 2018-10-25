using System;

namespace Linq2Ldap.Types
{
    public class LdapString
    {
        private string S;
        public LdapString(string s)
        {
            S = s;
        }

        public static implicit operator string(LdapString ls) => ls.ToString();

        public static implicit operator LdapString(string s) => new LdapString(s);

        public override string ToString() => S;
    }
}