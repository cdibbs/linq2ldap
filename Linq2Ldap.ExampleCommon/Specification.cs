using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Linq2Ldap.ExampleCommon.Specifications
{
    /// <summary>
    /// A specification is an expression that given an object of type T, produces
    /// true if the object satisifies the expression and false otherwise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Specification<T>: IEquatable<Specification<T>>
    {
        private Expression<Func<T, bool>> _expression;

        /// <summary>
        /// Used to record debugging information about the spec.
        /// </summary>
        public object Metadata { get; set; }

        public static Specification<T> All() => True;
        public static Specification<T> True => Start(t => true, true);
        public static Specification<T> None() => False;
        public static Specification<T> False => Start(t => false, false);

        public static Specification<T> Start(Expression<Func<T, bool>> expression, object Metadata = null)
        {
            return new Specification<T> { _expression = expression, Metadata = Metadata};
        }

        public Expression<Func<T, bool>> AsExpression()
        {
            return _expression.Expand();
        }

        public virtual Specification<T> And(Specification<T> spec)
        {
            return And(spec.AsExpression());
        }

        public virtual Specification<T> And(Expression<Func<T, bool>> expression)
        {
            var invokedExpr = Expression.Invoke(expression, _expression.Parameters.Cast<Expression>());
            _expression = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(_expression.Body, invokedExpr), _expression.Parameters);
            return this;
        }

        public virtual Specification<T> Or(Specification<T> spec)
        {
            return Or(spec.AsExpression());
        }

        public virtual Specification<T> Or(Expression<Func<T, bool>> expression)
        {
            var invokedExpr = Expression.Invoke(expression, _expression.Parameters.Cast<Expression>());
            _expression = Expression.Lambda<Func<T, bool>>(Expression.OrElse(_expression.Body, invokedExpr),
                                                           _expression.Parameters);
            return this;
        }

        public override int GetHashCode()
        {
            return AsExpression().Expand().ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Specification<T>);
        }

        public bool Equals(Specification<T> other)
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

        public static implicit operator Func<T, bool>(Specification<T> spec)
            => spec.AsExpression().Compile();

        public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
            => spec.AsExpression();

        public static implicit operator Specification<T>(Expression<Func<T, bool>> expr)
            => Specification<T>.Start(expr) as Specification<T>;
    }
}
