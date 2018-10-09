using System;
using System.Linq.Expressions;

namespace Specifications
{
    public interface ISpecification<T>
    {
        object Metadata { get; set; }
        Expression<Func<T, bool>> AsExpression();
        ISpecification<T> And(ISpecification<T> spec);
        ISpecification<T> And(Expression<Func<T, bool>> expression);
        //ISpecification<T> And(MethodCallExpression expression); // see implementation
        ISpecification<T> Or(ISpecification<T> spec);
        ISpecification<T> Or(Expression<Func<T, bool>> expression);
        //ISpecification<T> Or(MethodCallExpression expression); // see implementation
    }
}
