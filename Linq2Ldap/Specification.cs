using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Specifications
{
    /// <summary>
    /// A specification is an expression that given an object of type T, produces
    /// true if the object satisifies the expression and false otherwise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Specification<T> : ISpecification<T>, IEquatable<ISpecification<T>>
    {
        private Expression<Func<T, bool>> _expression;

        /// <summary>
        /// Used to record debugging information about the spec.
        /// </summary>
        public object Metadata { get; set; }

        public static ISpecification<T> All() => True;
        public static ISpecification<T> True => Start(t => true, true);
        public static ISpecification<T> None() => False;
        public static ISpecification<T> False => Start(t => false, false);

        public static ISpecification<T> Start(Expression<Func<T, bool>> expression, object Metadata = null)
        {
            return new Specification<T> { _expression = expression, Metadata = Metadata};
        }

        public Expression<Func<T, bool>> AsExpression()
        {
            return _expression.Expand();
        }

        public ISpecification<T> And(ISpecification<T> spec)
        {
            return And(spec.AsExpression());
        }

        public ISpecification<T> And(Expression<Func<T, bool>> expression)
        {
            var invokedExpr = Expression.Invoke(expression, _expression.Parameters.Cast<Expression>());
            _expression = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(_expression.Body, invokedExpr), _expression.Parameters);
            return this;
        }

        // Not sure this is ever used. Removing until proved wrong. CRD 2017-09-08.
        /*
        public ISpecification<T> And(MethodCallExpression expression)
        {
            _expression = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(_expression.Body, expression),
                                                           _expression.Parameters);
            return this;
        }*/

        public ISpecification<T> Or(ISpecification<T> spec)
        {
            return Or(spec.AsExpression());
        }

        public ISpecification<T> Or(Expression<Func<T, bool>> expression)
        {
            var invokedExpr = Expression.Invoke(expression, _expression.Parameters.Cast<Expression>());
            _expression = Expression.Lambda<Func<T, bool>>(Expression.OrElse(_expression.Body, invokedExpr),
                                                           _expression.Parameters);
            return this;
        }

        // Not sure this is ever used. Removing until proved wrong. CRD 2017-09-08.
        /*public ISpecification<T> Or(MethodCallExpression expression)
        {
            _expression = Expression.Lambda<Func<T, bool>>(Expression.OrElse(_expression.Body, expression),
                                                           _expression.Parameters);
            return this;
        }*/

        public override int GetHashCode()
        {
            return AsExpression().Expand().ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ISpecification<T>);
        }

        public bool Equals(ISpecification<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            var thisExpression = AsExpression().Expand().ToString();
            var otherExpression = other.AsExpression().Expand().ToString();
            return thisExpression == otherExpression;
        }
    }
}
