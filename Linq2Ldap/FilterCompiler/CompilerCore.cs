using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Linq2Ldap.Attributes;

namespace Linq2Ldap.FilterCompiler
{
    public class CompilerCore
    {
        internal StringComparisons StringComparisons { get; set; }
        internal SubstringOperations SubstringOperations { get; set; }
        internal BooleanAlgebra BooleanAlgebra { get; set; }

        public CompilerCore(
            StringComparisons stringComparisons = null,
            BooleanAlgebra booleanAlgebra = null,
            SubstringOperations substringOperations = null
        )
        {
            StringComparisons = stringComparisons ?? new StringComparisons(this);
            SubstringOperations = substringOperations ?? new SubstringOperations(this);
            BooleanAlgebra = booleanAlgebra ?? new BooleanAlgebra(this);
        }

        /// <summary>
        /// Recursively translates an Expression to an LDAP filter (RFC 1960).
        /// See formal definition of RFC 1960 at the bottom of this Microsoft doc page.
        /// https://docs.microsoft.com/en-us/windows/desktop/adsi/search-filter-syntax
        /// </summary>
        /// <param name="expr">The Expression body to convert.</param>
        /// <param name="p">The ParameterExpression associated with the body.</param>
        /// <returns>A string containing an equivalent LDAP filter.</returns>
        public string ExpressionToString(Expression expr, IReadOnlyCollection<ParameterExpression> p)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return _MemberToString(expr as MemberExpression, p);
                case ExpressionType.Constant:
                    return _ConstExprToString(expr, p);
                case ExpressionType.AndAlso:
                    return BooleanAlgebra.AndOrExprToString(expr, p, "&");
                case ExpressionType.OrElse:
                    return BooleanAlgebra.AndOrExprToString(expr, p, "|");
                case ExpressionType.Not:
                    return BooleanAlgebra.NotExprToString(expr, p);
                case ExpressionType.Equal:
                    return _MaybeStringCompareToExpr(expr, p, "=")
                           ?? _ComparisonExprToString(expr, p, "=");
                case ExpressionType.GreaterThanOrEqual:
                    return _MaybeStringCompareToExpr(expr, p, ">=")
                           ?? _ComparisonExprToString(expr, p, ">=");
                case ExpressionType.LessThanOrEqual:
                    return _MaybeStringCompareToExpr(expr, p, "<=")
                           ?? _ComparisonExprToString(expr, p, "<=");
                case ExpressionType.Call:
                    return _CallExprToString(expr, p);

                // These are not implemented in RFC 1960, so translate via negation.
                case ExpressionType.GreaterThan:
                    return _Negate(_MaybeStringCompareToExpr(expr, p, "<=", ">")
                            ?? _ComparisonExprToString(expr, p, /* not */ "<="));
                case ExpressionType.LessThan:
                    return _Negate(_MaybeStringCompareToExpr(expr, p, ">=", "<")
                           ?? _ComparisonExprToString(expr, p, /* not */ ">="));
                case ExpressionType.NotEqual:
                    return _Negate(_MaybeStringCompareToExpr(expr, p, /* not */ "=")
                           ?? _ComparisonExprToString(expr, p, /* not */ "="));

                // Low-priority/do not implement:
                case ExpressionType.Conditional: /* ternary */
                default:
                    throw new NotImplementedException(
                        $"Linq-to-LDAP-filter-string not implemented for {expr.NodeType}. \n"
                        + "Use local variables to remove algebraic expressions and method calls, \n"
                        + "and reduce Linq expression complexity to boolean algebra and \n"
                        + ".Contains/.StartsWith/.EndsWith string ops.");
            }
        }

        internal string _CallExprToString(
            Expression expr, IReadOnlyCollection<ParameterExpression> p)
        {
            var e = expr as MethodCallExpression;
            var name = e.Method.Name;
            var type = e.Method.DeclaringType;
            var fullname = $"{type}.{name}";
            var validSubExprs = new List<ExpressionType>()
                { ExpressionType.Constant, ExpressionType.MemberAccess};
            const string propertiesBagGetItem
                = nameof(Linq2Ldap) + "." + nameof(ResultPropertyCollectionProxy) + ".get_Item";
            switch (fullname)
            {
                case "System.String.Contains":
                case "Linq2Ldap.Types.LDAPStringList.Contains":
                case "Linq2Ldap.ResultPropertyValueCollectionProxy.Contains":
                    return SubstringOperations.OpToString(e, p, validSubExprs, "({0}=*{1}*)");
                case "Linq2Ldap.ResultPropertyValueCollectionProxy.StartsWith":
                case "Linq2Ldap.Types.LDAPStringList.StartsWith":
                case "System.String.StartsWith":
                    return SubstringOperations.OpToString(e, p, validSubExprs, "({0}={1}*)");
                case "Linq2Ldap.ResultPropertyValueCollectionProxy.EndsWith":
                case "Linq2Ldap.Types.LDAPStringList.EndsWith":
                case "System.String.EndsWith":
                    return SubstringOperations.OpToString(e, p, validSubExprs, "({0}=*{1})");
                case propertiesBagGetItem:
                    return __PDictIndexToString(e, p);
                default:
                    throw new NotImplementedException(
                        $"Linq-to-LDAP method calls only implemented for substring comparisons" +
                        $" (.Contains, .StartsWith, .EndsWith). Was: {fullname}.");
            }
        }

        internal string __PDictIndexToString(
            MethodCallExpression expr,
            IReadOnlyCollection<ParameterExpression> p)
        {
            var keyExpr = expr.Arguments[0];
            switch (keyExpr)
            {
                case ConstantExpression e when e.Type == typeof(string):
                    return e.Value as string;
                case MemberExpression _:
                    return EvalExpr(keyExpr, p);
            }

            throw new NotImplementedException(
                $"LDAP property reference must be a constant string. Was: {expr.Arguments[0].NodeType} / {keyExpr?.Type}");
        }

        public string EvalExpr(
            Expression expr, IReadOnlyCollection<ParameterExpression> p)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Constant:
                    return _ConstExprToString(expr, p);
                case ExpressionType.MemberAccess:
                    var objectMember = Expression.Convert(expr, typeof(object));
                    var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                    var getter = getterLambda.Compile();
                    return getter().ToString();
                default:
                    throw new NotImplementedException(
                        $"Linq-to-LDAP value access not implemented for type {expr.NodeType}.");
            }
        }

        internal string _Negate(string exprStr) => $"(!{exprStr})";

        internal string _ComparisonExprToString(
            Expression expr, IReadOnlyCollection<ParameterExpression> p, string op)
        {
            var e = expr as BinaryExpression;
            var left = ExpressionToString(e.Left, p);
            var right = EvalExpr(e.Right, p);
            return $"({left}{op}{right})";
        }

        internal string _MaybeStringCompareToExpr(
            Expression expr, IReadOnlyCollection<ParameterExpression> p, string op, string origOp = null)
        {
            origOp = origOp ?? op;
            var e = expr as BinaryExpression;
            MethodCallExpression[] mces;
            if ((mces = StringComparisons.IsStringCompare(e.Left, e.Right)).Any())
            {
                return StringComparisons.StringCompareToExpr(mces, e, p, op, origOp);
            }

            return null;
        }

        internal MemberExpression __IsParamModelAccess(Expression e, IReadOnlyCollection<ParameterExpression> p)
        {
            if (e is MemberExpression me && me.Expression == p.FirstOrDefault())
            {
                return me;
            }
            else if (e is UnaryExpression ue
                     && ue.NodeType == ExpressionType.Convert
                     && ue.Operand is MemberExpression ume
                     && ume.Expression == p.FirstOrDefault())
            {
                return ume;
            }

            return null;
        }

        internal string _ConstExprToString(
            Expression expr, IReadOnlyCollection<ParameterExpression> p)
        {
            var e = expr as ConstantExpression;
            if (e.Type != typeof(string) && e.Type != typeof(char))
            {
                throw new NotImplementedException(
                    $"Type {e.Type} not implemented in {nameof(_ConstExprToString)}.");
            }

            return e.Value.ToString();
        }

        internal string _MemberToString(
            MemberExpression me, IReadOnlyCollection<ParameterExpression> p)
        {
            if (__IsParamModelAccess(me, p) != null)
            {
                var attr = me.Member.GetCustomAttribute<LDAPFieldAttribute>();
                return attr != null ? attr.Name : me.Member.Name;
            }

            // We could eval it, but may be out of scope?
            throw new NotImplementedException($"Out-of-scope member expression: {me.Member.Name}.");
        }
    }
}
