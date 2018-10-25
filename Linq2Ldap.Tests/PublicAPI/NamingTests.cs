using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Linq2Ldap.FilterCompiler;

namespace Linq2Ldap.Tests.PublicAPI
{
    public class NamingTests {
        private static Assembly assembly;
        static NamingTests() {
            assembly = Assembly.GetAssembly(typeof(LdapFilterCompiler));
        }

        [MemberData(nameof(LdapRefs))]
        [Theory]
        public void LdapReferences_MustBeCamelCase(Type t) {
            var expectedPtrn = new Regex("Ldap");
            Assert.Matches(expectedPtrn, t.Name);
        }

        public static IEnumerable<object[]> LdapRefs
            => assembly.GetTypes()
                .Where(t =>
                    (t.IsClass || t.IsInterface || t.IsEnum)
                    && new Regex("ldap", RegexOptions.IgnoreCase).IsMatch(t.Name))
                .Select(t => new object[] { t });

        [MemberData(nameof(LinqRefs))]
        [Theory]
        public void LinqReferences_MustBeCamelCase(Type t) {
            var expectedPtrn = new Regex("Linq");
            Assert.Matches(expectedPtrn, t.Name);
        }

        public static IEnumerable<object[]> LinqRefs
            => assembly.GetTypes()
                .Where(t =>
                    (t.IsClass || t.IsInterface || t.IsEnum)
                    && new Regex("linq", RegexOptions.IgnoreCase).IsMatch(t.Name))
                .Select(t => new object[] { t });
    }
}