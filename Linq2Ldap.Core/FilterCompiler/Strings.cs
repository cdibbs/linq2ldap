using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Linq2Ldap.Core.FilterCompiler
{
    public class Strings
    {
        private CompilerCore Core { get; }
        public Strings(CompilerCore core)
        {
            Core = core;
        }

        public MethodCallExpression[] IsStringCompare(Expression left, Expression right)
        {
            var results = new MethodCallExpression[2];
            results[0] = left as MethodCallExpression;
            results[1] = right as MethodCallExpression;
            return results.Any(r => r != null && __IsStrCmp(r))
                ? results
                : new MethodCallExpression[0];
        }

        private bool __IsStrCmp(MethodCallExpression mce)
            => mce != null && (mce.Method.DeclaringType.FullName == "System.String"
                || mce.Method.DeclaringType.FullName == "Linq2Ldap.Types.LdapStringList")
               && (mce.Method.Name == "Compare" || mce.Method.Name == "CompareTo");

        public string StringCompareToExpr(
            MethodCallExpression[] mces,
            BinaryExpression expr, IReadOnlyCollection<ParameterExpression> p, string op, string origOp)
        {
            (MethodCallExpression compare, Expression compareVal, bool isReverse)
                = __IsStrCmp(mces[0]) ? (mces[0], mces[1] ?? expr.Right, false) : (mces[1], mces[0] ?? expr.Left, true);
            var val = (int)((ConstantExpression)compareVal).Value;
            var args = compare.Arguments;
            if (compare.Method.Name == "Compare")
            {
                return _NormalizeStringCompareAsString(val, args[0], args[1], p, op, origOp, isReverse);
            }

            // .CompareTo
            return _NormalizeStringCompareAsString(val, compare.Object, args[0], p, op, origOp, isReverse);
        }

        public string ExtensionOpToString(
            MethodCallExpression e, IReadOnlyCollection<ParameterExpression> p,
            string op)
        {
            IEnumerable<Expression> pair;
            if (e.Arguments.Count > 2
                || (pair = e.Arguments.Take(2)).Any(a => a == null))
            {
                throw new NotImplementedException(
                    $"Linq-to-LDAP string comparisons must have two, non-null parameters.");
            }

            return _NormalizeStringCompareAsString(0, pair.ElementAt(0), pair.ElementAt(1), p, op, op, false, false);
        }

        internal string _NormalizeStringCompareAsString(
            int val, Expression leftExpr, Expression rightExpr,
            IReadOnlyCollection<ParameterExpression> p, string op, string origOp, bool isReverse, bool escape = true)
        {
            string left = null, right = null;
            var me = Core.__IsParamModelAccess(leftExpr, p);
            var me2 = Core.__IsParamModelAccess(rightExpr, p);
            if (me2 == null && me != null)
            {
                left = Core._MemberToString(me, p);
                right = escape ? Core.EvalExpr(rightExpr, p) : Core.RawEvalExpr(rightExpr, p);
                op = _AdjustCompares(op, origOp, val, isReverse);
            }
            else if (me2 != null && me == null)
            {
                left = Core._MemberToString(me2, p);
                right = escape ? Core.EvalExpr(leftExpr, p) : Core.RawEvalExpr(leftExpr, p);
                op = _AdjustCompares(op, origOp, val, !isReverse);
            }
            else
            {
                throw new NotImplementedException("One and only one term of a .Compare/.CompareTo must reference the model.");
            }

            return $"({left}{op}{right})";
        }

        /// <summary>Use val and positional data to put member access always on left side.</summary>
        internal string _AdjustCompares(string op, string origOp, int val, bool reverse)
        {
            var reverseCompares = new Dictionary<string, string>()
            {
                { "<=", ">=" },
                { ">=", "<=" },
                { "=", "=" },
                { "~=", "~=" }
            };
            var nval = Math.Sign(val);

            // [op][nval] -> translated op
            var opValLookups = new Dictionary<string, Dictionary<int, string>>()
            {
                {"<=", new Dictionary<int, string>() {{-1, "<"}, {0, "<="}, {1, "NA"}}},
                {">=", new Dictionary<int, string>() {{-1, "NA"}, {0, ">="}, {1, ">"}}},
                {"=", new Dictionary<int, string>() {{-1, "<"}, {0, "="}, {1, ">"}}},
                {"~=", new Dictionary<int, string>() {{-1, "~="}, {0, "~="}, {1, "~="}}},

                // Some of these (marked /* ! */) will be negated for RFC 1960 compat.
                {"<", new Dictionary<int, string>() {{-1, "NA"}, {0, /* ! */ ">="}, {1, "<="}}},
                {">", new Dictionary<int, string>() {{-1, ">="}, {0, /* ! */ "<="}, {1, "NA"}}},
            };

            var trans = opValLookups[origOp][nval];
            return reverse ? reverseCompares[trans] : trans;
        }

        public string OpToString(
            MethodCallExpression e, IReadOnlyCollection<ParameterExpression> p,
            List<ExpressionType> validSubExprs,
            string subExpTmpl)
        {
            Expression firstArg;
            if (e.Arguments.Count > 1 || (firstArg = e.Arguments.FirstOrDefault()) == null)
            {
                throw new NotImplementedException(
                    $"Linq-to-LDAP string comparisons must have one parameter. Had: {e.Arguments.Count}.");
            }

            if (!validSubExprs.Contains(firstArg.NodeType))
            {
                throw new NotImplementedException(
                    $"Linq-to-LDAP string op arguments must be const string/variable."
                    + $" Was: {firstArg.NodeType}/{firstArg.Type}.");
            }

            var o = Core.ExpressionToString(e.Object, p);
            var c = Core.EvalExpr(firstArg, p);
            return String.Format(subExpTmpl, o, c);
        }
    }
}
