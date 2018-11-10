using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Linq2Ldap.Core.FilterCompiler;

namespace Linq2Ldap.Tests.PublicAPI
{
    public class NamingTests {
        private static Assembly assembly;
        static NamingTests() {
            assembly = Assembly.GetAssembly(typeof(LinqDirectorySearcher<>));
        }

        [MemberData(nameof(RefsFinder), "linq")]
        [Theory]
        public void LinqReferences_MustBeCamelCase(Type t) {
            var expectedPtrn = new Regex("Linq");
            Assert.Matches(expectedPtrn, t.Name);
        }

        public static IEnumerable<object[]> RefsFinder(string regex)
            => assembly.GetTypes()
                .Where(t =>
                    (t.IsClass || t.IsInterface || t.IsEnum)
                    && new Regex(regex, RegexOptions.IgnoreCase).IsMatch(t.Name))
                .Select(t => new object[] { t });
    }
}