using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Linq2Ldap.FilterCompiler
{
    public class SubstringOperations
    {
        protected CompilerCore Core { get; set; }
        public SubstringOperations(CompilerCore core)
        {
            Core = core;
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
                    $"Linq-to-LDAP string op param must be const string/variable."
                    + $" Was: {firstArg.NodeType}/{firstArg.Type}.");
            }

            var o = Core.ExpressionToString(e.Object, p);
            var c = Core.EvalExpr(firstArg, p);
            return String.Format(subExpTmpl, o, c);
        }
    }
}
