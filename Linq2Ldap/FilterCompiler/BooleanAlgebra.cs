using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Linq2Ldap.FilterCompiler
{
    public class BooleanAlgebra
    {
        protected CompilerCore Core { get; set; }
        public BooleanAlgebra(CompilerCore core)
        {
            Core = core;
        }

        internal string AndOrExprToString(
            Expression expr, IReadOnlyCollection<ParameterExpression> p, string op)
        {
            var e = expr as BinaryExpression;
            var left = Core.ExpressionToString(e.Left, p);
            var right = Core.ExpressionToString(e.Right, p);
            return $"({op}{left}{right})";
        }

        internal string NotExprToString(
            Expression expr, IReadOnlyCollection<ParameterExpression> p)
        {
            var e = expr as UnaryExpression;
            var operand = Core.ExpressionToString(e.Operand, p);
            return $"(!{operand})";
        }
    }
}
