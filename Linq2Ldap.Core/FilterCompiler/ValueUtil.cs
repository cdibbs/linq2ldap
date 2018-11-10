using System.Linq;
using System.Text.RegularExpressions;

namespace Linq2Ldap.Core.FilterCompiler {
    public class ValueUtil {
        public string EscapeFilterValue(string value) {
            var replacer = new Regex("([*\\\\#+<>;\"=])");
            var result = replacer.Replace(value, @"\$1");
            var left = result.TrimStart(' ');
            var right = result.TrimEnd(' ');
            var middle = result.Trim(' ');
            left = string.Concat(Enumerable.Repeat(@"\ ", result.Length - left.Length));
            if (middle.Length == 0) {
                return left;
            }
            right = string.Concat(Enumerable.Repeat(@"\ ", result.Length - right.Length));
            return left + middle + right;
        }
    }
}